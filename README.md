<div align="center">

<img src="https://capsule-render.vercel.app/api?type=wave&color=auto&height=250&section=header&text=ScreenDimmer&fontSize=80" />

![License](https://img.shields.io/badge/license-MIT-blue.svg?style=for-the-badge)
![.NET](https://img.shields.io/badge/.NET-8.0-512bd4.svg?style=for-the-badge&logo=dotnet&logoColor=white)
![Platform](https://img.shields.io/badge/platform-Windows-0078d7.svg?style=for-the-badge&logo=windows&logoColor=white)

夜中に突然開いた「Google翻訳」の真っ白な画面が眩しすぎて「ぎゃあ！」となる…
本ツールは「ディスプレイの輝度を物理設定からいじるのが面倒」で、
「眩しさに耐えかねた」作者が、半ギレで制作したものです。

<details>
<summary>English (Click to expand)</summary>

Ever found yourself blinded by the brilliant white screen of "Google Translate" in the dead of night, letting out a silent "Agh!"?
This tool was born out of pure frustration. The author, fed up with the blinding glare and too lazy to fiddle with physical monitor settings, created this in a fit of pique.
</details>

</div>

---

## 目次 (Table of Contents)
- [1. 概要 (Overview)](#1-概要-overview)
- [2. 特徴 (Features)](#2-特徴-features)
- [3. 技術的アーキテクチャ (Technical Architecture)](#3-技術的アーキテクチャ-technical-architecture)
- [4. 使い方 (Usage)](#4-使い方-usage)

---

## 1. 概要 (Overview)
ScreenDimmerは、Windows 10/11環境において、メインモニターの輝度をソフトウェア側で擬似調整する常駐型ツールです。
メイン画面全体に全ての操作を無視する黒いオーバーレイを生成し、擬似的にディスプレイの輝度を下げることができます。
シンプルかつ確実に暗くしたかったので、インストール不要で即座に利用可能です。
お気に召したらスタートアップフォルダにショートカットを追加するなどして適宜ご利用ください。

<details>
<summary>English (Click to expand)</summary>

ScreenDimmer is a lightweight, portable utility for Windows 10/11 that simulates brightness adjustment on your primary monitor.
It generates a black, click-through overlay that covers the entire main screen, effectively lowering the perceived brightness while ignoring all user inputs.
Designed for simplicity and reliability, it requires no installation. If you find it useful, feel free to add a shortcut to your Startup folder for daily use.
</details>

<br>

https://github.com/user-attachments/assets/aa0f9a43-45f7-4d2c-afcb-d631691c31ef

## 2. 特徴 (Features)
* **単一ファイル実行** : DLLやランタイムに依存せず、 `.exe` 一つでどこでも動作します。
* **仮想デスクトップ対応** : デスクトップを切り替えても、暗転フィルターが外れたり移動したりすることなく、常にメインモニターに常駐します。
* **低負荷・非破壊** : レジストリを汚染せず、OSの描画レイヤーに透明なフィルターを重ねるだけの安全な設計です。

## 3. 技術的アーキテクチャ (Technical Architecture)
このツールがどのように動作しているか、技術的な構造に興味がある方は以下を参照してください。

### オーバーレイウィンドウ (Overlay Window)
* `WindowState="Maximized"` を利用し、プライマリディスプレイ（メインモニター）全体を覆う仮想的なウィンドウを生成します。
* `WindowStyle="None"`, `AllowsTransparency="True"`, `Topmost="True"` の組み合わせにより、タイトルバーのない最前面フィルターを実現しています。
* Win32 API（ `SetWindowLong` ）を用いて `WS_EX_TRANSPARENT` および `WS_EX_LAYERED` を付与し、マウスクリックなどの入力を背後のアプリケーションへ完全に透過させます。

### 仮想デスクトップの透過 (Virtual Desktop Persistence)
* 拡張スタイルに `WS_EX_TOOLWINDOW` (0x00000080) を付与することで、OSに対して「ツール属性」を宣言しています。
* これにより、仮想デスクトップの切り替えに関わらず、指定されたモニター上で継続して描画が維持されるよう設計されています。

### 排他制御 (Concurrency Control)
* `System.Threading.Mutex` を利用した単一インスタンス保証を実装し、多重起動によるリソース競合を防止しています。

## 4. 使い方 (Usage)
1. [Releases](https://github.com/qajoule/ScreenDimmer/releases) から最新の `ScreenDimmer.exe` をダウンロードします。
2. ダブルクリックで実行すると、タスクトレイにアイコンが表示されます。
3. **左クリック**: フィルターのON/OFFを切り替えます。
4. **右クリック > Settings**: 透過度（暗さ）を調整します。
5. **右クリック > Exit**: アプリを終了します。

## 5. 免責事項・ライセンス (License)

本ソフトウェアは、私(椎名一霤)による趣味のプロジェクトであり、MITライセンスの下で公開されています。
本ツールの利用によって生じた損害等について、作者は一切の責任を負いません。自己責任において目の安寧のためにご利用ください。
<div align="center">
© 2026 椎名一霤
</div>
