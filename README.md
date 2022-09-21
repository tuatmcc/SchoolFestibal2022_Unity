# 動作環境
* Windows 11
* Unity Editor 2021.3.7f1

# 概要
プレイヤー名を入力するTitleScene、レースの行われるMainScene、ゴール後に結果を表示するResultScene、これらのシーン間のデータを持つためのManagerSceneでできています。ManagerSceneは初めに読み込まれ、残り続けます。

InputSystemsとCinemachine, UniTaskのパッケージを追加でインポートしています。

# 注意
* RaceManager.csはMonoBehaviourを継承していません。シーン上のオブジェクトを参照するためにRaceManagerTrigger.csというクラスを使っています。
* PlayerとEnemyのプレハブの子にArmatureというプレハブがあります。シーン上で各キャラクターを横並びにさせるために、Enemyをシーンに配置後Armatureをローカル座標でx方向に2ずつずらしています。
