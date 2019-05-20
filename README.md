# HoloMagnet3

![HoloMagnet with English Subtitles](https://user-images.githubusercontent.com/129954/56087679-a65bdc00-5eaa-11e9-8f3d-f08724833a32.gif)
![2019年4月14日：HoloMagnet36説明動画](https://user-images.githubusercontent.com/129954/56087721-73feae80-5eab-11e9-9aca-0b4f8a344ed6.gif)

## What's this?

- This is a Science (Physics) education app for the newest Mixed Reality (MR) technology headset **"HoloLens"**. 
- The purpose of this app is for **education**. The target of this app is students in junior high school, high school, college and university.
- You can learn about magnetic field which can **not** be seen in real world.
- This is one of education app which is **few** in the world.
- Everyone can get this app via **Microsoft Store** and experience.

## Concept

- Implementation example of proper size for understanding how to develop for HoloLens Development **newbies**
- Simple and easy UI for a user who have never touch HoloLens and experience for the **first time**
- Example of **MVP Design pattern** as a HoloLens application
- Example **avoiding** `GameObject.Find()` which slow down the process speed

## Getting Started

1. **Clone** this repository
2. **Open** the project
3. **Build**
4. **Deploy** to HoloLens
5. **Start** app named "HoloMagnet3"

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

## Development Environment

- Unity 2017.4.26f1
- Visual Studio 2017.15

## Licence

MIT

## Contact

Email: tatsuro.ueda@feel-physics.jp

Twitter: [\@feelphysicsjp](https://twitter.com/feelphysicsjp)

Homepage: https://www.youtube.com/watch?v=1nC2X5o88xA&list=PLUUoZRXFtWCeUb-ZHFwxF1LjgrtBOYd3G

Facebook: https://www.facebook.com/feelphysicsjp

## これは何？

- 最先端の複合現実ヘッドセット **「HoloLens」** 用の理科（物理）学習アプリです。
- このアプリの目的は**教育**です。対象は中学生、高校生、専門学校生、大学生です。
- このアプリを使って、現実世界では目で**見ることのできない**磁界について学習することができます。
- **数少ない**教育用HoloLensアプリです。
- **誰でも**Microsoft Storeで無料で入手して体験することが可能です。

## 特徴

- **初心者**HoloLens開発者にとって、開発方法を学びやすい手軽な規模の実装例
- HoloLensを体験するのが**初めてのユーザ**でも簡単に操作できるシンプルなUI
- HoloLensアプリケーションのデザインパターンとして**MVPパターン**を採用した例
- Unityの処理が落ちる`GameObject.Find()`を極力**使用しない**例

## 始め方

1. このリポジトリを**クローン**します
2. Unityで、プロジェクトを**開きます**
3. **ビルド**します
4. HoloLensに**配置**します
4. 「HoloMagnet3」を**起動**します

## 使い方

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

## ストアへの登録

公式の説明はここに書かれています：

[Windows アプリを公開する \- Windows UWP applications \| Microsoft Docs](https://docs.microsoft.com/ja-jp/windows/uwp/publish/)

また、[ゆーじ様の書かれたHoloLens向けのアプリ公開の方法](http://2vr.jp/2017/01/30/submitting-an-app-to-the-windows-store/#i-3)で大変丁寧に解説されています。

以下では少しわかりにくい箇所を説明します

### アプリのアイコン画像を生成し、登録する

以前はこの機能がなく、[高橋忍様が開発されたフリーソフト](https://blogs.msdn.microsoft.com/shintak/2015/08/22/uwp-logo-maker-ver-1-0/)に頼り切りでした。

現在はVisual Studio上でアイコンを自動生成することができます。

「Project」→「Store」→「Edit App Manifest」→「Visual Assets」を開きます。

![Untitled - Paint 2019-05-20 12 13 36](https://user-images.githubusercontent.com/129954/57994582-cbbab600-7af8-11e9-9646-41fb1eed3410.png)

「Source」でアイコン画像を選択します。このとき注意したいのが、200KB以下の画像を選択して下さい。さもないと、あとでビルドするときにエラーが出ます。

![HoloMagnet3-190114 - Microsoft Visual Studio 2019-05-20 10 24 57](https://user-images.githubusercontent.com/129954/57993904-e17aac00-7af5-11e9-9288-fdd9546600cb.png)

「Apply recommended padding」をオフにします。さもないと、アイコン画像をうまく枠にフィットしてくれません。

![HoloMagnet3-190114 - Microsoft Visual Studio 2019-05-20 10 53 49](https://user-images.githubusercontent.com/129954/57994161-c9eff300-7af6-11e9-835a-37eefcdccb39.png)

「Generate」をクリックするとアイコン画像が生成されます。

![HoloMagnet3-190114 - Microsoft Visual Studio 2019-05-20 11 00 48](https://user-images.githubusercontent.com/129954/57994208-002d7280-7af7-11e9-819b-d0b3d5ae8ad8.png)

### Splash Logoを変える

Splash Logoというのは、これです。

![8f40ad8804d452a96150f3eeaf4637e0](https://user-images.githubusercontent.com/129954/57995993-fceab480-7aff-11e9-9c04-fb86a386b56b.png)

これだけだと寂しいので、自社のロゴを追加しましょう。

まず、正方形のpngファイルをAssetsに入れます。

![Unity 2017 4 25f1 Personal (64bit) - Introduction unity - HoloMagnet3 - Universal Windows Platform _DX11_ 2019-05-20 13 07 58](https://user-images.githubusercontent.com/129954/57996059-56eb7a00-7b00-11e9-8392-cb1cafaaaa1d.png)

「Texture Type」を「Sprite(2D and UI)」にします。

![Unity 2017 4 25f1 Personal (64bit) - Introduction unity - HoloMagnet3 - Universal Windows Platform _DX11_ 2019-05-20 13 08 44](https://user-images.githubusercontent.com/129954/57996094-88644580-7b00-11e9-98b5-b5db7e56cd3c.png)

「Sprite Mode」→「Mesh Type」を「Full Rect」にします。

![Unity 2017 4 25f1 Personal (64bit) - Introduction unity - HoloMagnet3 - Universal Windows Platform _DX11_ 2019-05-20 13 18 25](https://user-images.githubusercontent.com/129954/57996339-d0d03300-7b01-11e9-837b-c36971e144a0.png)

「Edit」→「Project Settings」→「Player」

![Untitled - Paint 2019-05-20 12 16 23](https://user-images.githubusercontent.com/129954/57994654-23592180-7af9-11e9-82dc-65e95aebd967.png)

「Splash Image」→「Logos」→「+」

![Unity 2017 4 25f1 Personal (64bit) - Introduction unity - HoloMagnet3 - Universal Windows Platform _DX11_ 2019-05-20 13 11 32](https://user-images.githubusercontent.com/129954/57996160-d7aa7600-7b00-11e9-97de-e41db1303ee7.png)

ここで先ほど追加したロゴの画像ファイルを選択します。

少し上の「Splash Screen」→「Preview」を見てみましょう。

![2019年5月20日：スプラッシュロゴを追加する](https://user-images.githubusercontent.com/129954/57996825-a59b1300-7b04-11e9-8f32-e0699151612a.gif)

Splash Logoが変わったことがわかります。

## 開発環境

- Unity 2017.4.26f1
- Visual Studio 2017.15

## ライセンス
MIT

## 連絡先

Email: tatsuro.ueda@feel-physics.jp

Twitter: [@feelphysicsjp](https://twitter.com/feelphysicsjp)

Homepage: https://www.youtube.com/watch?v=1nC2X5o88xA&list=PLUUoZRXFtWCeUb-ZHFwxF1LjgrtBOYd3G

Facebook: https://www.facebook.com/feelphysicsjp

© 2019 Feel Physics All rights reserved.