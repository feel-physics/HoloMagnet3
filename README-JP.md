[English](README.md) / 日本語

## HoloMagnet3

本アプリは、**「de:code 2019」**（マイクロソフト社の開発者をはじめとするITに携わるすべてのエンジニアのための年に一度のテクニカルカンファレンス）において、

**「Microsoft MVP アワード」**（マイクロソフトの製品やテクノロジーに関する豊富な知識や経験を他者と共有することで、すべてのユーザーが最大限に製品を活用できるよう多大なサポートをおこなったコミュニティのリーダーに、マイクロソフトが感謝の意を表して授与する賞）の受賞者として、

セッション内容をより深く理解し実践するのに役に立つコード「[パターン](#構成図)を用い、シンプルな [UI](#UI・表現) を提供する、初心者でもできる HoloLens アプリ開発と [Microsoft ストアへの登録方法](#ストア登録)～実際の[ソースコード](#軽量化)と[構成図](#構成図)を見ながら～」という位置づけで公開したものです。

- このアプリは [5カ国10の学校300人の体験者](#授業風景) のフィードバックにより作られました。
- 最先端の複合現実ヘッドセット **「HoloLens」** 用の理科（物理）学習アプリです。
- このアプリの目的は**教育**です。対象は中学生、高校生、専門学校生、大学生です。
- このアプリを使って、現実世界では目で**見ることのできない**磁界について学習することができます。
- **数少ない**教育用HoloLensアプリです。
- **誰でも** [Microsoft ストアで無料で入手](https://www.microsoft.com/ja-jp/p/holomagnet3/9pff2nq2t708)して体験することができます。

## 目次

- [特徴](#特徴)
  - [軽量化](#軽量化)
  - [UI・表現](#UI・表現)
  - [論文](#論文)
- [構成図](#構成図)
- [ビルド方法](#ビルド方法)
- [謝辞](#謝辞)

## 特徴

### 軽量化

**500個**の方位磁針の個々の物理計算を毎フレームおこないながら、FPS **58**を実現しています。

具体的には、シェーダをインスタンス化**せず**に、シェーダ**内**で物理計算をおこなって負荷を大幅に減らしています。

なお、軽量化が必要な理由については次項の「UI・表現」で説明します。

### シェーダをインスタンス化しない

以下のように、マテリアルをスクリプトから操作せずに、シェーダに外部変数を受け取る変数を設定し、スクリプトからマテリアルを介さずにシェーダに**直接**値を代入しています。

[CompassesManagedlySimultaneouslyUpdater\.cs](https://github.com/feel-physics/HoloMagnet3/blob/master/Assets/HoloMagnet3/Prefabs/CompassesManager/CompassesManagedlySimultaneouslyUpdater.cs#L140-L150)

```csharp
void AssignMagnetPosition()
    {
        var np = barMagnet01NorthPole.transform.position;
        var sp = barMagnet01SouthPole.transform.position;
        var nv4 = new Vector4(np.x, np.y, np.z, 0);  //Vector4 に変換
        var sv4 = new Vector4(sp.x, sp.y, sp.z, 0);  //Vector4 に変換

        // 方位磁針の N 極側のマテリアルのシェーダに座標をセット
        CompassesModel.Instance.MatNorth.SetVector("_NorthPolePos", nv4);
        CompassesModel.Instance.MatNorth.SetVector("_SouthPolePos", sv4);
        // 方位磁針の S 極側のマテリアルのシェーダに座標をセット
        CompassesModel.Instance.MatSouth.SetVector("_NorthPolePos", nv4);
        CompassesModel.Instance.MatSouth.SetVector("_SouthPolePos", sv4);
    }
```

これによりシェーダがインスタンス化されず、**単一**のシェーダとして処理されるため、計算負荷を大幅に減らすことができます。

#### シェーダ内で物理計算を行う

以下のように、シェーダ内で物理計算を行っています。

[MyCompassShader2\.shader](https://github.com/feel-physics/HoloMagnet3/blob/master/Assets/HoloMagnet3/Resources/Compass180509/MyCompassShader2.shader#L49-L77)

```ShaderLab
// 自身（方位磁針）の位置ベクトルvecPを作成
float3 vecP;
vecP = IN.worldPos;

// N極の位置ベクトルvecNを作成
float3 vecN;
vecN.x = _NorthPolePos.x;
vecN.y = _NorthPolePos.y;
vecN.z = _NorthPolePos.z;

// S極の位置ベクトルvecSを作成
float3 vecS;
vecS.x = _SouthPolePos.x;
vecS.y = _SouthPolePos.y;
vecS.z = _SouthPolePos.z;

// 自身から棒磁石に対する変位ベクトルvecDisN、vecDisSを作成
float3 vecDisN, vecDisS;
vecDisN = vecP - vecN;
vecDisS = vecP - vecS;

// 極からの磁力ベクトルvecF_N, vecF_Sを求める
float3 vecF_N, vecF_S;
vecF_N =        vecDisN / pow(length(vecDisN), 3);
vecF_S = -1.0 * vecDisS / pow(length(vecDisS), 3);

// 磁力の合力ベクトルvecFを求める
float3 vecF;
vecF = vecF_N + vecF_S;
```

これにより、物理計算をGPU内で完結させることができ、CPUへの負荷を大幅に減らすことができます。HoloLensのCPUはとても非力なため、必要な処理をどれだけGPUに回せるかが重要です。

### UI・外見

本アプリを用いた授業は、5カ国の10の学校で300人が体験しました。全員のアンケート（定量および定性）を収集し、それらを元に、アプリ体験をより良くし学習効果と満足度を上げるために、以下の工夫をおこないました。

- 操作が安定していない（例えばフォーカスが外れる、マーカートラッキングがしばしば途切れる）と、体験者は**不満**を感じます。初めて体験するユーザに対するHoloLensアプリは、**極めて簡易**な操作方法を提供することが望ましいです。

- 動作が**遅い**と、体験者は**不満**を感じるため、動作を可能な限り速くすることが望ましいです。
- **シェアリング**は高次元の体験を提供することができますが、途中で接続が切れた場合の復旧に時間がかかるため、時間にタイトな授業（40人の場合、3分のロスも許されません）には**適しません**。

#### 表現

- プロジェクターで多人数に見せるためには、表示するオブジェクトは**大きく**するべきです。
- 理解を妨げるため、体験とプロジェクターに映された映像のあいだの**タイムラグ**は短くするべきです。
- 従来の**抽象的な表現**（磁力線など）だけでは初学者にとって分かりにくいため、別の理解を助ける**表現の工夫**（例えば格子状に配置された方位磁針）が必要です。
- **静**的な表現よりも**動**的な表現の方が理解しやすく、身体を動かしてコンテンツを**体験**することができるとユーザの理解度と満足度は高くなります。

#### 論文

[5カ国での拡張現実技術を用いた磁界体験の実践（査読論文）](https://www.jstage.jst.go.jp/article/pesj/67/4/67_266/_article/-char/ja/)

## 構成図

本アプリの構成図です。

![StructureDiagram](https://user-images.githubusercontent.com/129954/58148478-dc566200-7c99-11e9-89ed-abb2a20f726b.png)

クラス関係とアクティビティフローを両方表現するために、シンプルな独自の構成図を描きました。

1. 大きな円がプレハブ、中くらいの円がクラス、小さい円がメソッドを表しています。
2. 矢印が、ユーザからのアクションがどのような経路をたどって処理されるかを表しています。

プレハブに対するクラスの役割は以下の通りです。

- プレハブの上部にあるクラスがプレハブのハンドラ
- プレハブの右側にあるクラスがプレハブのモデル
- プレハブの下側にあるクラスがプレハブのコントローラ

HoloLensアプリを作成する際は、最初はどうしても無秩序にプレハブやクラスを作ってしまいがちです。

しかし、プレハブごとにハンドラ、モデル、コントローラクラスを作成すると、見通しの良い設計を手軽におこなうことができます。

## ビルド方法

### 環境

- Unity 2017.4.26f1（2017.4系列なら動くでしょう）
- Visual Studio 2017.15 (すべての Windows 10 SDK にチェックして下さい)
![Visual Studio Installer 2019-06-19 16 59 55](https://user-images.githubusercontent.com/129954/59748279-c0080e00-92b5-11e9-9854-9ecda68a4f90.png)

1. このリポジトリを**クローン**します
2. Unityで、プロジェクトを**開きます**
3. ビルド先フォルダ（「UWP」とするのが一般的）を指定して**ビルド**します
4. ビルドが終わるとプロジェクトフォルダがエクスプローラによって開かれるので、先ほど指定したフォルダを**開きます**
4. 「HoloMagnet3.sln」ファイルを**開きます**
4. Visual Studio を使って HoloLensに**配置**します
4. 「HoloMagnet3」を**起動**します

### 操作

- **棒磁石を動かす**
  ホールドしながら腕を動かすことで棒磁石を動かすことができます
- **磁力線を表示**
  タップします
- **次のシーンに進む**
  ダブルタップする
  - シーンは4つあります
  - 最後のシーンでダブルタップすると最初のシーンに戻ります

1. 棒磁石の移動に**慣れて**もらうためのシーン（棒磁石しかない）
2. **1つ**の磁界を表すコンパスのあるシーン
3. **平面グリッド**上にコンパスが並んでいるシーン
4. **立体グリッド**上にコンパスが並んでいるシーン

## 謝辞

- 鈴鹿高校 田畑雅基先生
- 三重高校 村田了祐先生、川田博基先生
- 愛知総合工科高校 川田大介先生
- 学芸大附属高校 大西啄也先生
- 津東高校 佐野哲也先生
- St. Giles Cambridge 校 Philip 先生
- JICA ボランティア 杉本憲広博士
- JICA ボランティア 酒井章宏先生
- 神戸市アフリカビジネスミッションコーディネーター Samuel IMANISHIMWE 様
- 神戸情報大学院大学 Business Development 学部長 Nick Barua 博士
- トゥンバ工科大学 MUTABAZI Rita Clémence 校長
- ルワンダ大学理工学部情報工学科 Santhi Kumaran 助教
- 林知樹様
- 松井幸治様
- シェアオフィス「ビズスクエアよっかいち」のスタッフの皆様
- 日本・イギリス・ガーナ・ルワンダ・US の 300 人以上のフィードバックを下さった生徒たち

## 連絡先

[Email](mailto:tatsuro.ueda@feel-physics.jp), 
[Homepage](https://feel-physics.jp),
[Facebook](https://www.facebook.com/feelphysicsjp),
[Twitter](https://twitter.com/feelphysicsjp),
[LinkedIn](https://www.linkedin.com/in/tatsuro-ueda/)
