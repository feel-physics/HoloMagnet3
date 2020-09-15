English / [日本語](README-JP.md)

# HoloMagnet3

Physics education app visualizes Magnetic Field for HoloLens

This app can be used with HoloLens 2.

Any **teacher** can use this app in your classrooms, 
and any **researcher** can use this app to write your paper.

We are very welcome to your contribution **for education** including porting to other platforms (such as Magic Leap One, Oculus Quest, iPad, Pixel3, and many others).

![2019年5月22日：HoloMagnet37、3次元自動](https://user-images.githubusercontent.com/129954/58151375-74f1df80-7ca4-11e9-89c6-a6a0fb16346f.gif)
![2018年6月21日：（学会発表用）三重高校愛知総合工科高校授業風景320x180](https://user-images.githubusercontent.com/129954/58155580-1bdb7900-7caf-11e9-896a-229f64b4f12a.gif)

[日本語](#日本語)

## Overview

The app was presented at de:code 2019 (an annual technical conference for Microsoft developers and all engineers involved in IT) .

As winner of the "Microsoft MVP Award" (which recognizes a wealth of knowledge and experience with Microsoft products and technology, and community leaders who have gone above and beyond to help all users get the most out of the product by sharing it with others”, the code is positioned as "How to develop HoloLens apps with [patterns](#Configuration-Diagram) and a simple [UI](#UI--Expression) for beginners.

- This app was created with feedback from **300 testers from 10 schools in 5 countries**.
-	This is a science (physics) learning app for the HoloLens, a state-of-the-art Mixed Reality (MR) headset.
-	The purpose of this app is education. It is intended for junior high school, high school, vocational school, college students and science museum.
- With this app, you can learn about magnetic fields that you can't see with your eyes in the real world.
- **this is one of the few HoloLens apps for education.**
- anyone can get it and experience it for free at [the Microsoft Store](https://www.microsoft.com/en-us/p/holomagnet3/9pff2nq2t708).

---

## Table of Contents

- [Characteristics](#Characteristics)
  - [Light Load](#Light-Load)
  - [UI / Expression](#UI--Expression)
  - [Thesis](#Thesis)
- [Configuration Diagram](#Configuration-Diagram)
- [Build Method](#Build-Method)
- [Operation](#Operation)
- [Acknowledgements](#Acknowledgements)

## Characteristics

### Light Load

![2019年5月22日：HoloMagnet37、2次元](https://user-images.githubusercontent.com/129954/58151322-4411aa80-7ca4-11e9-9fca-fdcaf8baae17.gif)
![2019年5月22日：HoloMagnet37、3次元手動](https://user-images.githubusercontent.com/129954/58151979-496ff480-7ca6-11e9-92f8-cfecf742ec68.gif)

**The FPS 58 is achieved while calculating the physics of each of the 500 magnetic compasses on each frame.**

Specifically, we do not instantiate the shaders, but perform physical calculations within them to significantly reduce the load.

The reason for the need to reduce weight is explained in the next section, [UI / Expressions](#UI--Expression).

### Do not instantiate the shader

As shown below, instead of manipulating the material from the script, we set up a variable that takes an external variable in the shader and then assign the value directly from the script to the shader without involving the material.

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

This greatly reduces the computational load, as the shaders are not instantiated and are processed as a single shader.

#### Performing Physical Computations in a Shader

The following is a physics calculation in the shader.

[MyCompassShader2\.shader](https://github.com/feel-physics/HoloMagnet3/blob/master/Assets/HoloMagnet3/Resources/Compass180509/MyCompassShader2.shader#L49-L77)

```ShaderLab
// Create position vector of vecP of itself (magnetic compass)
float3 vecP;
vecP = IN.worldPos;

// Create position vector vecN for the North-pole
float3 vecN;
vecN.x = _NorthPolePos.x;
vecN.y = _NorthPolePos.y;
vecN.z = _NorthPolePos.z;

// Create a position vector vecS for the South-pole
float3 vecS;
vecS.x = _SouthPolePos.x;
vecS.y = _SouthPolePos.y;
vecS.z = _SouthPolePos.z;

// Create displacement vectors vecDisN and VecDisS for a bar magnet from itself
float3 vecDisN, vecDisS;
vecDisN = vecP - vecN;
vecDisS = vecP - vecS;

// Find the magnetic force vector vecF_N and vecF_S from the poles
float3 vecF_N, vecF_S;
vecF_N =        vecDisN / pow(length(vecDisN), 3);
vecF_S = -1.0 * vecDisS / pow(length(vecDisS), 3);

// Find the magnetic force vector vecF
float3 vecF;
vecF = vecF_N + vecF_S;
```

This allows the physical computation to be completed within the GPU, greatly reducing the load on the CPU; since the HoloLens CPU is so impractical, it is important to make sure that the GPU can do all the necessary work.

### UI / Expression

![2019年5月22日：HoloMagnet37、3次元磁力線](https://user-images.githubusercontent.com/129954/58152110-9d7ad900-7ca6-11e9-9be9-31caa7a0c727.gif)

This app was used in 10 schools in 5 countries, with 300 students experiencing the lessons. We collected both quantitative and qualitative questionnaires from all the students, and based on them, we made the following efforts to improve their experience and increase their learning effectiveness and satisfaction with the app.

#### UI

- If the operation is inconsistent (e.g., out of focus, marker tracking is often interrupted), the experience is **frustrating** for the user. HoloLens apps for first-time users should provide extremely **simple controls**.
- It is advisable to make the process **as fast as possible**, as slow **process** will frustrate the experience.
- **Sharing** can provide a high-dimensional experience, but it's **not suitable** for time-tight classes (no more than 3 minutes can be lost for 40 students) because of the time it takes to recover if the connection is lost in the middle of the class.

#### Expression

- In order to show the projector to a large number of people, the objects to be displayed **should be large**.
- The **time lag** between the experience and the image on the projector should be shortened to hinder understanding.
- **Conventional abstract expressions** (e.g., magnetic force lines, etc.) alone are difficult for the beginner to understand, and so we need to devise an **alternative expression** (e.g., magnetic compasses arranged in a grid pattern) to aid understanding.
- **Dynamic representations** are easier to understand than **static representations**, and users are more likely to understand and be satisfied when they are able to **physically experience** the content.

#### Lesson Scenes

The followings are lesson / session scenes. The reason why the appearance of the app wasn't same between them is, because we improved it step by step.

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

#### Thesis

[Practicing Magnetic Field Experiences Using Augmented Reality Technology in Five Countries (Refereed Paper, Japanese)](https://www.jstage.jst.go.jp/article/pesj/67/4/67_266/_article/-char/ja/)

## Configuration Diagram

This is the structure of the application.

![StructureDiagram](https://user-images.githubusercontent.com/129954/58148478-dc566200-7c99-11e9-89ed-abb2a20f726b.png)

(Drawn with [Qt Visual Graph Editor](https://github.com/ArsMasiuk/qvge))

We drew a simple, unique structure diagram to represent both class relationships and activity flow.

1. The large circle represents the prefab, the medium circle represents the class, and the small circle represents the method.
2. The arrows represent the path the user's action will take.

The role of the class against prefab is as follows

- The class at the top of the prefab is the prefab handler
- The class to the right of the prefab is the prefab model
- The class underneath the prefab is the controller of the prefab

When creating a HoloLens app, it's easy to start out with a disorderly prefab or class.

However, creating handlers, models, and controller classes for each prefab makes it easier to get a good perspective on the design.

## Build Method

### Environment

- Unity 2019.4
- Visual Studio 2019 (Check all Windows 10 SDKs)
![Visual Studio Installer 2019-06-19 16 59 55](https://user-images.githubusercontent.com/129954/59748279-c0080e00-92b5-11e9-9854-9ecda68a4f90.png)

### Procedure

1. **Clone** this repository
2. **Open** your project with Unity
3. Specify the destination folder("UWP" is the most common name) and **build**
3. When the build is finished, the project folder will be opened by explorer and you can **open** the folder you just specified.
3. **Open** "HoloMagnet3.sln".
4. **Deploy** to HoloLens using Visual Studio
5. **Launch** "HoloMagnet3"

## Operation

The followings are for HoloLens 1. For HoloLens 2, we have prepared two buttons for operation so that it's very easy to operate.

- **Move the bar magnet**
  You can move the bar magnet by moving your arm while holding it
- **Display lines of magnetic force**
  Tap
- **Move on to the next scene**
  Double-tap
  - There are four scenes
  - Double tap on the last scene to go back to the first scene

1. A scene to **get you used** to moving a bar magnet (there is only a bar magnet)
2. A scene with a compass representing a **single** magnetic field
3. Scene with compasses on a **planar grid**
4. Scene with compasses lined up on a **three-dimensional grid**

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
- All staffs in the share office BizSquare Yokkaichi
- 300 students who gave me their feedbacks in Japan, UK, Ghana, Rwanda, US

## Contact

[Email](mailto:tatsuro.ueda@feel-physics.jp), 
[Homepage](https://feel-physics.jp),
[Facebook](https://www.facebook.com/feelphysicsjp),
[Twitter](https://twitter.com/feelphysicsjp),
[LinkedIn](https://www.linkedin.com/in/tatsuro-ueda)

---

© 2019 Feel Physics® All rights reserved.
