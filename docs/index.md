---
title: Home
---
![Latest release](https://img.shields.io/github/v/release/mattwhitfield/VisualFindReferences?color=00A000) ![Last commit](https://img.shields.io/github/last-commit/mattwhitfield/VisualFindReferences?color=00A000) ![Build status](https://img.shields.io/github/workflow/status/mattwhitfield/VisualFindReferences/Extension%20build) ![Open issue count](https://img.shields.io/github/issues/mattwhitfield/VisualFindReferences)

# Introduction 👀
The Visual Find References Visual Studio extension adds a new, graphical reference finding experience to Visual Studio. If you are trying to understand the code flow through a new code base, and find Call Hierarchy a bit limiting, this extension is for you.

* Presents references on a node graph display
* Allows you to extend the graph simply and easily from the visual display
* Allows you to filter which projects you find references in, so you can ignore test projects while trying to understand main code flow, for example.
* Provides simple visual cues and automatic layout.

### Documentation Sections 📖

* [Getting started](gettingstarted.md) - covers basic usage.
* [Contributing](contributing.md) - covers useful information for anyone who wants to contribute to the project.

## Supported Environments 🌳

### Visual Studio Versions 🆚
Due to the transition to 64-bit, Visual Studio 2022 introduces some architectural differences that necessitate a separate VSIX package. If you're working with Visual Studio 2019, you will need [Visual Find References for Visual Studio 2019](https://marketplace.visualstudio.com/items?itemName=MattWhitfield.VisualFindReferences) and if you're working with Visual Studio 2022, you will need [Visual Find References for Visual Studio 2022](https://marketplace.visualstudio.com/items?itemName=MattWhitfield.VisualFindReferencesVS2022).

## Contributing ✋

Any contributions are welcome. Code. Feedback on what you like or what could be better. Please feel free to fork the repo and make changes, the license is MIT so you're pretty much free to do whatever you like. For more information, please see the [contributing section](contributing.md). You can get started by visiting the [Visual Find References GitHub repo](https://github.com/mattwhitfield/VisualFindReferences).

## Support 🤝

If you'd like to support the project but you don't want to contribute code - a really good way to help is spread the word. There isn't a donation or financial option becuase honestly I don't need donations. I am, however, rubbish at 'marketing'. So any help there would be greatly appreciated! Whether it's a blog post, a marketplace review or just telling some folks at the office - every little helps.

## Thanks ❤

This project wouldn't be possible without other MIT projects that I used to base my graph code from. Many thanks to the [NodeGraph](https://github.com/lifeisforu/NodeGraph) and [GraphShape](https://github.com/KeRNeLith/GraphShape) projects.