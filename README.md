# 動作環境
* Windows 11
* Unity Editor 2021.37f1

# 概要
プレイヤー名を入力するTitleScene、レースの行われるMainScene、ゴール後に結果を表示するResultScene、これらのシーン間のデータを持つためのManagerSceneでできています。ManagerSceneは初めに読み込まれ、残り続けます。

InputSystemsとCinemachineのパッケージを追加でインポートしています。

## ManagerSceneAutoLoader.cs
ManagerSceneを初めに読み込むクラス。どこにもアタッチしていません。

## SceneLoader.cs
シーンの遷移とローディング画面を管理するクラス。ManagerSceneのルートオブジェクトについています。

## RaceManager.cs
プレイヤー名の保持、レースの開始・終了、順位の管理をするクラス。ManagerSceneのルートオブジェクトについています。

## CharacterControll.cs
プレイヤーにもエネミーにもついているクラス。PlayerController.csとEnemyController.csから操作します。

## PlayerController.cs
レース時のプレイヤーの入力を受け取ります。クリック、タップ、何かしらのキーで加速します。

## EnemyController.cs
エネミー側の入力に相当します。加速頻度(確率)と最高速度制限で強さを決めています。

## GeneratePathMesh.cs
コースとなるCinemachineSmoothPathをミニマップで可視化するためのメッシュジェネレーター。

## MenuSceneManager.cs
TitleSceneのCanvas/Background/PlayerNameSpaceについています。クラス名間違えた

## LogoDisplay
TitleSceneのCanvas/LogoBackgroundについています。

## CountDownDisplay.cs
MainSceneのCanvas/CountDown/CowntDownImageについています。RaceManagerのカウントダウンを読みます。

## ResultDisplayManager.cs
ResultSceneのルートオブジェクトについています。順位表の表示名を管理します。

## CommonConst.cs
定数に楽にアクセスしたかったんですが、自分でも何をやっているかわかりません。

