# lilToon Perspective Correction

A modified version of [lilToon](https://github.com/lilxyzw/lilToon) with optional camera-relative perspective correction for anime-style character faces.

This fork is based on **lilToon 2.3.4**.

# Installation

### Unity Package Manager

Open:

```text
Window → Package Manager → + → Install package from git URL
```

Enter
```
https://github.com/reiware/lilToon.git?path=Assets/lilToon#perspective-correction
```

## Setup

1. Add the `Perspective Corrector` component to the root object of the character.
2. Assign the character's head bone.
3. Assign the renderers that should receive the correction.
4. Optionally assign a specific target camera.
5. Enable **Perspective Correction** on the affected lilToon materials.
6. Adjust the correction settings.

## Comparison With Original lilToon

View the complete diff from lilToon 2.3.4:

[Compare lilToon 2.3.4 with the perspective-correction branch](https://github.com/reiware/lilToon/compare/2.3.4...perspective-correction)

## Original Project

This repository is based on [lilToon](https://github.com/lilxyzw/lilToon), created by [lilxyzw](https://github.com/lilxyzw).

For the original shader documentation, supported features, setup instructions, and changelog, visit the official lilToon repository:

[Official lilToon Repository](https://github.com/lilxyzw/lilToon)

## License

The original lilToon project and these modifications are distributed under the MIT License.

Original lilToon copyright:

```text
Copyright (c) 2020-present lilxyzw
```

Perspective correction modifications:

```text
Copyright (c) 2026 reiware
```