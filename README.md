# HoloMagnet3

Physics education app visualizes Magnetic Field for HoloLens

Any **teacher** can use this app in your classrooms, 
and any **researcher** can use this app to write your paper.

We are very welcome to your contribution **for education** including porting to other platforms (such as Magic Leap One, Oculus Quest, iPad, Pixel3, and many others).

![2019年5月22日：HoloMagnet37、3次元自動](https://user-images.githubusercontent.com/129954/58151375-74f1df80-7ca4-11e9-89c6-a6a0fb16346f.gif)
![2018年6月21日：（学会発表用）三重高校愛知総合工科高校授業風景320x180](https://user-images.githubusercontent.com/129954/58155580-1bdb7900-7caf-11e9-896a-229f64b4f12a.gif)

[日本語](#日本語)

## Overview

This code is published to **help** understanding sessions of HoloLens app development in IT Tech conference which is held once a year by Microsoft Japan. 

The reason why I published this open-source app is, because I was one of 17 personal sponsors who have **Microsoft MVP Award** (The Microsoft **M**ost **V**aluable **P**rofessional award is given by Microsoft to "technology experts who passionately share their knowledge with the community.")

- Made by feedbacks of **300** experienced people in **5** countries at **11** schools
- Physics education app for headset with mixed reality technology (**very new** technology).
- Objective is experience education for:
  - **student** in junior high school, high school, college, university, night school
  - visitor in science **museum**, event
- You can see invisible phenomena, magnetic field, which can **not** be seen in real world. 
- **Rare** education app for mixed reality headset.
- **Anyone** can get app via [Microsoft Store](https://www.microsoft.com/en-us/p/holomagnet3/9pff2nq2t708)
- **Free**
---

## TOC

- [Characteristics](#Characteristics)
  - [Light Load](#Light-Load)
  - [UI / Expression](#UI--Expression)
  - [Paper](#Paper)
- [Structure](#Structure)
- [How to Build](#How-to-Build)
- [Acknowledgements](#Acknowledgements)

## Characteristics

### Light Load

![2019年5月22日：HoloMagnet37、2次元](https://user-images.githubusercontent.com/129954/58151322-4411aa80-7ca4-11e9-9fca-fdcaf8baae17.gif)
![2019年5月22日：HoloMagnet37、3次元手動](https://user-images.githubusercontent.com/129954/58151979-496ff480-7ca6-11e9-92f8-cfecf742ec68.gif)

The app executes the Physics calculation of each of **500** compasses on every frame, and the FPS is **58**.

Specifically, it does **not** instantiate it's Shader. It executes Physics calculation in the single Shader. As a result, the load is very light.

Furthermore, the reason why the light weight is needed is explained in next subject "UI / Expression".

### Does not instantiate shader

As the following codes, the Material is not operated by a Script. Valuables are set in the Shader which accept external valuables. They are assigned **directly** to the Shader by a Script.

[CompassesManagedlySimultaneouslyUpdater\.cs](https://github.com/feel-physics/HoloMagnet3/blob/master/Assets/HoloMagnet3/Prefabs/CompassesManager/CompassesManagedlySimultaneouslyUpdater.cs#L140-L150)

```csharp
void AssignMagnetPosition()
    {
        var np = barMagnet01NorthPole.transform.position;
        var sp = barMagnet01SouthPole.transform.position;
        var nv4 = new Vector4(np.x, np.y, np.z, 0);  // Convert to Vector4
        var sv4 = new Vector4(sp.x, sp.y, sp.z, 0);  // Convert to Vector4

        // Set coordinates to Shader of Material of NORTH side of compass
        CompassesModel.Instance.MatNorth.SetVector("_NorthPolePos", nv4);
        CompassesModel.Instance.MatNorth.SetVector("_SouthPolePos", sv4);
        // Set coordinates to Shader of Material of SOUTH side of compass
        CompassesModel.Instance.MatSouth.SetVector("_NorthPolePos", nv4);
        CompassesModel.Instance.MatSouth.SetVector("_SouthPolePos", sv4);
    }
```

As a result, the Shader is not instantiated and processed as a **single** Shader. That significantly reduces the calculation load.

#### Execute Physics calculation in Shader

As the following codes, Physics calculation is executed in a Shader.

[MyCompassShader2\.shader](https://github.com/feel-physics/HoloMagnet3/blob/master/Assets/HoloMagnet3/Resources/Compass180509/MyCompassShader2.shader#L49-L77)

```ShaderLab
// Define position vector of self (compass) as vecP
float3 vecP;
vecP = IN.worldPos;

// Define position vector of NORTH Pole as vecN
float3 vecN;
vecN.x = _NorthPolePos.x;
vecN.y = _NorthPolePos.y;
vecN.z = _NorthPolePos.z;

// Define position vector of SOUTH Pole as vecS
float3 vecS;
vecS.x = _SouthPolePos.x;
vecS.y = _SouthPolePos.y;
vecS.z = _SouthPolePos.z;

// Define displacement vector from self to bar magnet as vecDisN, vecDisS
float3 vecDisN, vecDisS;
vecDisN = vecP - vecN;
vecDisS = vecP - vecS;

// Get magnetic force vectors from two poles as vecF_N, vecF_S
float3 vecF_N, vecF_S;
vecF_N =        vecDisN / pow(length(vecDisN), 3);
vecF_S = -1.0 * vecDisS / pow(length(vecDisS), 3);

// Get resultant magnetic force vector as vecF
float3 vecF;
vecF = vecF_N + vecF_S;
```

As a result, Physics calculation is completed in GPU. It significantly reduces CPU load. Since the CPU power of HoloLens is very weak, it's **important** to transfer necessary processes to GPU.

### UI / Expression

![2019年5月22日：HoloMagnet37、3次元磁力線](https://user-images.githubusercontent.com/129954/58152110-9d7ad900-7ca6-11e9-9be9-31caa7a0c727.gif)

More than 300 people in 5 countries at places including 11 schools were **practically taken** experience lessons / sessions with this app. **Learning tests** were taken in 3 schools, and **all** of them gave **questionaires** (both quantitative and qualitative). These results made the app experience **better**, and made the learning efficience and satisfaction degree **increase**. The devices are as followings: 

#### UI

- Unstable operation (for example, Focus or marker tracking runs off) makes user feel dissatisfaction. It's desirable to provide **very simple operation** for a user who experience for the first time.
- Slowness makes user feel dissatisfaction. It's desirable to make it **as fast as possible**.
- Although **Sharing** provides a experience of higher level, it takes a few minutes to recover. Thus, it's **not suitable** for a lesson which is severe on time such as 40 students classroom.

#### Expression

- Objects to display **should be big** in order to show for many people with a projector or a big TV
- It's difficult to understand old abstract expression (such as Lines of Magnetic Force) for beginner. Thus, devices of expression to **help understanding** (such as compasses arranged in grid) are important.
- Static expression is more understandable than dynamic expression. Satisfaction and comprehension degrees are higher if an user can **move his/her body** to experience contents.

#### Lesson Scenes

The followings are lesson / session scenes. Appearance of the app was changing, because we improved step by step.

![2018年5月21日-大阪メイカーズバザール](https://user-images.githubusercontent.com/129954/58153296-a4efb180-7ca9-11e9-8455-55201ff63c64.jpg)
![170829-MFT](https://user-images.githubusercontent.com/129954/58153048-1844f380-7ca9-11e9-8ab6-206a428bd166.jpg)
![22908671_1328002897323117_732263550_o](https://user-images.githubusercontent.com/129954/58153422-eaac7a00-7ca9-11e9-87df-3fa1c4f5d337.jpg)
![2018年6月21日：（学会発表用）三重高校愛知総合工科高校授業風景320x180](https://user-images.githubusercontent.com/129954/58155580-1bdb7900-7caf-11e9-896a-229f64b4f12a.gif)
![2018年3月8日-京進スクールワン四日市ときわ教室320x180](https://user-images.githubusercontent.com/129954/58155734-770d6b80-7caf-11e9-8d47-9faaa2eac46d.gif)
![★2018年4月13日：障碍者ITカレッジ（明るくした）320x180](https://user-images.githubusercontent.com/129954/58155754-842a5a80-7caf-11e9-9898-6898d22c2c02.gif)
![2018年6月6日：津東高校v3-3-320x180](https://user-images.githubusercontent.com/129954/58161211-cd33dc00-7cba-11e9-8d16-91cc5fad74a9.gif)
![イギリスでのHoloLens授業](https://user-images.githubusercontent.com/129954/58163040-3d902c80-7cbe-11e9-81e2-cab23644c8ea.jpg)
![2018年10月22日：ガーナ教員研修（含生徒）短縮版360x180](https://user-images.githubusercontent.com/129954/58161677-a88c3400-7cbb-11e9-87d2-dce10115ecfb.gif)
![2018年10月26日：ガーナ、ホ工科大学授業320x180](https://user-images.githubusercontent.com/129954/58161826-f3a64700-7cbb-11e9-9052-d7153e02de74.gif)
![2018年11月10日：FabLab-Rwanda320x180](https://user-images.githubusercontent.com/129954/58161999-497aef00-7cbc-11e9-83a0-179c352b0c9d.gif)
![2018年11月13日：ルワンダビジネスマッチング320x180](https://user-images.githubusercontent.com/129954/58162136-8a730380-7cbc-11e9-9996-21cbfab1cc93.gif)
![2018年11月15日：tumba college of technology320x180](https://user-images.githubusercontent.com/129954/58162229-c4440a00-7cbc-11e9-9144-ffc5f7cd8f29.gif)
![2018年11月15日：University of Rwanda320x180](https://user-images.githubusercontent.com/129954/58162388-0e2cf000-7cbd-11e9-935c-8778c2bc9672.gif)
![2019年3月20日：グロサミ2019-320x180](https://user-images.githubusercontent.com/129954/58162591-6bc13c80-7cbd-11e9-9895-c03c71186d13.gif)

#### Paper

We were asked by The Physics Education Society of Japan to write a paper about these knowledge. We already posted. The detail is going to be put on the journal. We are going to add the link to the paper on publication.

## Structure

This is the structure figure of the app.

![StructureDiagram](https://user-images.githubusercontent.com/129954/58148478-dc566200-7c99-11e9-89ed-abb2a20f726b.png)

(Drawn with [Qt Visual Graph Editor](https://github.com/ArsMasiuk/qvge))

We drew simple original structure figure in order to express both class relations and activity flows.

1. Large circle means Prefab, Middle size circle means class, Small circle means method.
2. Arrows means paths of process from user

Rolls of classes against a Prefab is followings:

- Handler class is over the Prefab
- Model class is on the right of the Prefab
- Controller class is under the Prefab

A beginner developer of HoloLens tends to create classes disorderly.

However, when you distinguish handler, model and controller classes against each Prefab, you can design with good prospect easily.

## How to Build

### Environment

- Unity 2017.4.26f1 (2017.4.x seems to work)
- Visual Studio 2017.15 (Check all Windows 10 SDKs)
![Visual Studio Installer 2019-06-19 16 59 55](https://user-images.githubusercontent.com/129954/59748279-c0080e00-92b5-11e9-9854-9ecda68a4f90.png)

### Procedure

1. **Clone** this repository
2. **Open** the project with Unity
3. **Build** into target folder (normally named "UWP")
3. When the build finished, file explorer opens the project folder.
**Open** the target folder.
3. **Open** "HoloMagnet3.sln" with Visual Studio.
4. **Build -> Deploy** to HoloLens
5. **Start** app named "HoloMagnet3" in HoloLens


## Usage

- **Move Bar Magnet**
  Hold and move your arm, then the bar magnet moves
- **Show Magnetic Force Lines**
  Tap
- **Proceed to Next Scene**
  Double tap
  - There are four scenes
  - You can return to the first scene by double tap in the last scene

1. **Introduction** scene in which you can grow accustomed to movement of a bar magnet
2. **One compass** scene which shows the magnetic field
3. Arranged in **2-Dimension** compasses scene
4. Arranged in **3-Dimension** compasses scene

## Acknowledgements

- Susuka High School, Teacher Masaki Tabata
- Mie High School, Teacher Ryosuke Murata and Teacher Hiroki Kawada
- Prefectural Aichi High School of Technology and Engineering, Teacher Daisuke Kawada
- Tokyo Gakugei University Senior High School, Teacher Takuya Ohnishi
- Tsu Higashi High School, Teacher Tetsuya Sano
- St. Giles Cambridge School, Teacher Philip
- Japan International Cooperation Agency (JICA) volunteer, Dr. Norihiro Sugimoto
- Japan International Cooperation Agency (JICA) volunteer, Mr. Akihiro Sakai
- Kobe City Africa Business Mission Coordinator, Samuel IMANISHIMWE
- Kobe Institute of Computing Business Development Director, Dr. Nick Barua
- Integrated Polytechnic Regional College (IPRC) Tumba, Principal MUTABAZI Rita Clémence
- University of Rwanda Dept of Computer Engineering Associate Professor, Dr. Santhi Kumaran
- Tomoki Hayashi
- Koji Matsui
- All staffs in share office BizSquare Yokkaichi
- 300 students who gave me their feedbacks in Japan, UK, Ghana, Rwanda, US

## Contact

[Email](mailto:tatsuro.ueda@feel-physics.jp), 
[Homepage](https://feel-physics.jp),
[Facebook](https://www.facebook.com/feelphysicsjp),
[Twitter](https://twitter.com/feelphysicsjp),
[LinkedIn](https://www.linkedin.com/in/weed7777/)

---

## 日本語

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
- [ストア登録](#ストア登録)
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

これらの知見については日本物理教育学会から研究報告の執筆を依頼され、すでに投稿しました。学会誌に詳細が掲載される予定です。公開され次第リンクに変えます。

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

## ストア登録

公式の詳細な説明はここに書かれています：

[Windows アプリを公開する \- Windows UWP applications \| Microsoft Docs](https://docs.microsoft.com/ja-jp/windows/uwp/publish/)

（[ゆーじ様の書かれたHoloLens向けのアプリ公開の方法](http://2vr.jp/2017/01/30/submitting-an-app-to-the-windows-store/#i-3)でも丁寧に解説されています）

以下では、公式の説明に補足した方が良いと思われる事項について説明します

### アプリのアイコン画像を生成し、登録する

以前はこの機能がなく、[高橋忍様が開発されたフリーソフト](https://blogs.msdn.microsoft.com/shintak/2015/08/22/uwp-logo-maker-ver-1-0/)に頼り切りでした。

現在はVisual Studio上でアイコンを自動生成することができます。

「Project」→「Store」→「Edit App Manifest」→「Visual Assets」を開きます。

![Untitled - Paint 2019-05-20 12 13 36](https://user-images.githubusercontent.com/129954/57994582-cbbab600-7af8-11e9-9646-41fb1eed3410.png)

「Source」でアイコン画像を選択します。

![HoloMagnet3-190114 - Microsoft Visual Studio 2019-05-20 10 24 57](https://user-images.githubusercontent.com/129954/57993904-e17aac00-7af5-11e9-9288-fdd9546600cb.png)

「Apply recommended padding」をオフにします。さもないと、アイコン画像をうまく枠にフィットしてくれません。

![HoloMagnet3-190114 - Microsoft Visual Studio 2019-05-20 10 53 49](https://user-images.githubusercontent.com/129954/57994161-c9eff300-7af6-11e9-835a-37eefcdccb39.png)

「Generate」をクリックするとアイコン画像が生成されます。

![HoloMagnet3-190114 - Microsoft Visual Studio 2019-05-20 11 00 48](https://user-images.githubusercontent.com/129954/57994208-002d7280-7af7-11e9-819b-d0b3d5ae8ad8.png)

### スプラッシュ画像を変える

「Splash Screen」の画像だけを変更します。

![NotificationsForm 2019-05-20 14 50 43](https://user-images.githubusercontent.com/129954/57998986-b94b7700-7b0e-11e9-8f95-bb191d62c963.png)


### アプリアイコンを変える

スタートパネル上のアイコンをUnityのデフォルトのものから変えます。

まずエクスプローラでUnityプロジェクトフォルダ→「UWP」→「（プロジェクト名）」→「Assets」に移動し、「Square310x310Logo.scale-200.png」というファイルをUnityのプロジェクトのAssetsフォルダに入れます。

![Assets 2019-05-20 14 38 17](https://user-images.githubusercontent.com/129954/57998548-fe6ea980-7b0c-11e9-92d6-326c39747dab.png)

「Edit」→「Project Settings」→「Player」を開きます

![Untitled - Paint 2019-05-20 12 16 23](https://user-images.githubusercontent.com/129954/57994654-23592180-7af9-11e9-82dc-65e95aebd967.png)

「Icon」→「Univarsal 10 Tiles and Logos」→「Scale 200% (620x620 pixels)」を開き、先ほどプロジェクトに追加した画像ファイルを選択します。

![NotificationsForm 2019-05-20 14 47 06](https://user-images.githubusercontent.com/129954/57998870-362a2100-7b0e-11e9-8a9d-22b866eac054.png)

これでHoloLensのスタートパネルのアイコンを変更することができます。

### ストアに登録するためにビルドする

HoloLens2はARMアーキテクチャかもしれないので、「ARM」にもチェックを入れます。2つとも「Master」でビルドします。

![NotificationsForm 2019-05-20 15 05 06](https://user-images.githubusercontent.com/129954/57999542-b8b3e000-7b10-11e9-8660-b0f13901d71e.png)

以下のようなエラーでビルドが止まる場合は、該当のファイルのサイズをPhotoshopの「Web用に保存」などで小さくします。

![NotificationsForm 2019-05-20 14 55 40](https://user-images.githubusercontent.com/129954/57999205-88b80d00-7b0f-11e9-92f6-f4c73c5793e3.png)

### ストア上の情報を登録する

ビルドしてアップロードするとストア登録がだいたい終わった気になりますが、その後の作業量が意外に多いです。記入事項を事前に準備しておくと良いです。

---

まず、以下の事項を記入します。

- アプリ名
- アプリの説明
- （更新したときは）更新内容
- 検索キーワード

---

![2019年5月22日：ストア上の情報を登録するページのスクリーンショット_1](https://user-images.githubusercontent.com/129954/58159028-9a87e480-7cb6-11e9-9aff-c00786d1e511.png)

---

次に、スクリーンショットを登録します。

---

![2019年5月22日：ストア上の情報を登録するページのスクリーンショット_2](https://user-images.githubusercontent.com/129954/58159248-0b2f0100-7cb7-11e9-81de-5dd635947bae.png)

---

アプリのアイコンを登録します。

---

![2019年5月22日：ストア上の情報を登録するページのスクリーンショット_3](https://user-images.githubusercontent.com/129954/58159303-2f8add80-7cb7-11e9-9750-085b83f53e32.png)

---

ここがわかりにくいのですが、以下のように進めます。

1. トレーラー動画をアップロードする
2. 「Windows10 & XBox Image」をアップロードする
3. 少し上に戻って、ストアのトップに表示するトレーラーを選びます

2番目の事項をしないと、3番目の事項ができません。気をつけて下さい。

---

![2019年5月22日：ストア上の情報を登録するページのスクリーンショット_4](https://user-images.githubusercontent.com/129954/58159471-8ee8ed80-7cb7-11e9-84a6-fa0a8d1e1fff.png)

---

以下の事項を記入します。

- ユーザがストアでアプリを検索する際に、部分一致でもヒットするように文節で分けたアプリ名
- 音声認識で認識されるように文節で分けたアプリ名

---

![2019年5月22日：ストア上の情報を登録するページのスクリーンショット_5](https://user-images.githubusercontent.com/129954/58159754-20585f80-7cb8-11e9-8ffd-57891875231c.png)

---

以下の事項を記入します。

- サポートのメールアドレス
- 検索用キーワード
- 著作権と商標
- 開発者名

---

![2019年5月22日：ストア上の情報を登録するページのスクリーンショット_6](https://user-images.githubusercontent.com/129954/58160010-90ff7c00-7cb8-11e9-88de-e9bf28a8b2ea.png)

---

これが終わったら、Submit しましょう。

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
[LinkedIn](https://www.linkedin.com/in/weed7777/)

---

© 2019 Feel Physics® All rights reserved.