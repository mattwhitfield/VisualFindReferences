# Introduction 👀
The Visual Find References Visual Studio extension adds a new, graphical reference finding experience to Visual Studio. If you are trying to understand the code flow through a new code base, and find Call Hierarchy a bit limiting, this extension is for you.

* Presents references on a node graph display
* Allows you to extend the graph simply and easily from the visual display
* Allows you to filter which projects you find references in, so you can ignore test projects while trying to understand main code flow, for example.
* Provides simple visual cues and automatic layout.

For more in-depth documentation, visit the [documentation on GitHub Pages](https://mattwhitfield.github.io/VisualFindReferences/).

# Using the Extension 🔧

To get started using the extension, simply click the new item on the code editor context menu:

![Code editor context menu](https://raw.githubusercontent.com/mattwhitfield/VisualFindReferences/main/docs/assets/menuitem.png)

You will then start with a visual reference finding experience that makes navigating your code really simple:

![Graph display](https://raw.githubusercontent.com/mattwhitfield/VisualFindReferences/main/docs/assets/main_graph.png)

# Visual Studio Versions 🆚
Due to the transition to 64-bit, Visual Studio 2022 introduces some architectural differences that necessitate a separate VSIX package. If you're working with Visual Studio 2019, you will need [Visual Find References for Visual Studio 2019](https://marketplace.visualstudio.com/items?itemName=MattWhitfield.VisualFindReferences) and if you're working with Visual Studio 2022, you will need [Visual Find References for Visual Studio 2022](https://marketplace.visualstudio.com/items?itemName=MattWhitfield.VisualFindReferencesVS2022).

## Thanks ❤

This project wouldn't be possible without other MIT projects that I used to base my graph code from. Many thanks to the [NodeGraph](https://github.com/lifeisforu/NodeGraph) and [GraphShape](https://github.com/KeRNeLith/GraphShape) projects.
