English / [日本語](README-JP.md)

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
- [Usage](#Usage)
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

[Practices of Magnetic Field Experience Sessions with Augmented Reality technology in five countries (Reviewed, Japanese)](https://www.jstage.jst.go.jp/article/pesj/67/4/67_266/_article/-char/ja/)

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
[LinkedIn](https://www.linkedin.com/in/tatsuro-ueda)

---

© 2019 Feel Physics® All rights reserved.
