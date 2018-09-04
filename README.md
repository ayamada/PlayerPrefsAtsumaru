# PlayerPrefsAtsumaru

アツマール上に置いたUnityのWebGLゲームでセーブロードするやつ(の叩き台)


# これは何？

非公式ながら、UnityのWebGLゲームはRPGアツマール( https://game.nicovideo.jp/atsumaru/ )に置く事が可能です。

が、セーブとロードが動きません。

なんでかというと、RPGアツマール上でのセーブとロードは独自拡張された `localStorage` を使う(もしくはアツマールAPI直叩き)必要があるのですが、UnityのWebGLデプロイでの `PlayerPrefs` は `localStorage` ではなく `IndexedDB` を使ってセーブデータを処理しており、 `IndexedDB` はアツマール上では保存されない為です。

なので「 `localStorage` に保存する `PlayerPrefs` っぽい奴」を作りました。
それが `PlayerPrefsAtsumaru` です。
これを使えばアツマール上でもセーブロードできます。


# 試す

山田がアップロードした動作サンプルは今のところ、以下に置いてます。

https://game.nicovideo.jp/atsumaru/games/gm7735?key=6fe563c1c240

この動作サンプルのプロジェクト一式は github の「releases」のところからダウンロード可能です( https://github.com/ayamada/PlayerPrefsAtsumaru/releases 内の「Source code (zip)」より)。

Unityエディタでこの一式を開いて、エディタ上から動かしたり、WebGLビルドして動かしたりしてみてください。

また、アツマールに限定公開でアップロードして、セーブとロードが正常に動く事も確認できるでしょう。


# 自分のゲームに導入する

サンプルプロジェクト一式の中から、以下の二つのファイルを持っていき、自分のゲームのプロジェクト内に入れてください。

- `Assets/Plugins/FileIO.jslib`
- `Assets/PlayerPrefsAtsumaru.cs`

`FileIO.jslib` は必ず `Plugins/` の中に入れる必要があるようです。

あとは、 `PlayerPrefs` を使う部分で、代わりに `PlayerPrefsAtsumaru` を使ってください。

`PlayerPrefs` の使い方については、以下にリファレンスがあります。

- https://docs.unity3d.com/jp/current/ScriptReference/PlayerPrefs.html

また、あちこちに解説があると思うので、自分で探してもよいでしょう。

サンプルプロジェクト一式の中の `Assets/SampleScene.cs` にも実際のコードがあります。


# Q&A

- `PlayerPrefsAtsumaru.SetString()` とかしてもセーブされない
    - データの保存後は忘れずに `PlayerPrefsAtsumaru.Save()` を実行してください。このタイミングでアツマールのサーバと通信してオンライン保存されます。
    - 間隔を置かずに連続でセーブするとエラーになるようです(エラーと言っても例外が投げられる訳ではなく、ゲーム画面の右下あたりに「セーブに失敗しました」みたいなメッセージがアツマール側より表示される)。最低でも1秒ほど、可能なら10秒ほど空けて実行するようにした方がよいです。

- インスタンスとかDictionaryとかを保存したい
    - 使い方が `PlayerPrefs` と同じなので、検索すればやり方が出てくると思います。

- 他のアツマールのゲームみたいに、セーブデータを切り替えたい
    - 未対応です。今は一個固定です。自分でがんばって `PlayerPrefsAtsumaru.cs` をいじれば可能です。山田はそこまでの気力はないので、誰かがんばってください。

- 色々と改善してほしい
    - 山田はがんばっていじる気力がないので、githubの右上の「fork」ボタンを押して、あなたが改善してみてください。


# ライセンス

- `PlayerPrefsAtsumaru` 本体のライセンスはzlibライセンスとします。
    - 当ライブラリの利用時にcopyright文等を表示させる義務はありません。無表示で使えます。
    - zlibライセンスの日本語での解説は https://ja.wikipedia.org/wiki/Zlib_License 等で確認してください。
    - `PlayerPrefsAtsumaru` 本体以外のサンプルプロジェクト一式としては、後述のテンプレやunity由来の色々が含まれています。これらについては元々のライセンスを参照してください。


# その他の参考情報

- `PlayerPrefsAtsumaru`は、Robert Wahlerさんのgistの `localStorage` 保存コードを参考にしています(アツマール対応の為、保存処理はかなり違ったものになってますが) → https://gist.github.com/robertwahler/b3110b3077b72b4c56199668f74978a0

- しぐささんによる、Unityからのアツマールのコメント制御プラグイン → https://najicore.com/atsumaru.html

- 山田が書いた、結構前のまとめ記事 → http://rnkv.hatenadiary.jp/entry/2018/06/26/114904

- アツマールで動かした際にブラウザウィンドウを縮小すると画面が見切れる人は、↓のtemplateを導入すると直ります
    - テンプレ → https://seansleblanc.itch.io/better-minimal-webgl-template
    - テンプレ導入手順 → https://docs.unity3d.com/ja/current/Manual/webgl-templates.html
    - uGUIを使っている場合、uGUIのボタン類だけずれる状態になります。この場合は、uGUIのCanvasの「UI Scale Mode」を「Scale With Screen Size」に設定し、「Reference Resolution」を画面サイズに設定するとよいです。uGUIを使っていなければこの設定は不要です
        - METAL BRAGEさんによる、uGUI用設定解説記事 → http://www.metalbrage.com/UnityTutorials/uGUI/Scaler.html


# 更新情報

- 2018/09/01 v0.1.0
    - githubにて公開

- 2018/08/01
    - セーブデータ内に `Application.productName` を埋め込み、違うゲームのデータは読み込まないようにした
    - `Library/` は消してもよいらしいので、消して、ファイルサイズを大幅節約

- 2018/07/31
    - 初回リリース



