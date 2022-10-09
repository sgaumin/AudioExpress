# AudioExpress
[![Unity 2018.4+](https://img.shields.io/badge/unity-2018.4%2B-blue.svg)](https://unity3d.com/get-unity/download)
[![License: MIT](https://img.shields.io/badge/License-MIT-brightgreen.svg)](https://github.com/sgaumin/AudioExpress/blob/main/LICENSE.md)

Unity library to setup and play 2D sound in a more convenient way: no need to attach a AudioSource component, all is managed in the background.

## Installation
Simply import this package with Unity Package Manager by using the url *https://github.com/sgaumin/AudioExpress.git*

## System Requirements
Unity **2018.4** or later versions. Don't forget to include the AudioExpress namespace.

## Overview

### Inspector
From the inspector, we can assign audio clip and edit parameters

![Unity_HsQ9A4fd5Y](https://user-images.githubusercontent.com/16069763/194754562-21a9036e-b1dc-4176-9eda-061ce952f745.png)


### Scene View
In the scene, Audio Units are automatically created and rearranged under an object called AudioParent

![Unity_hNt8T6NLnu](https://user-images.githubusercontent.com/16069763/194754328-edc2d5b4-a147-4e91-80ed-a819ed4a50b2.png)


### Code Example
```csharp
using AudioExpress

public class PlayerController : MonoBehaviour
{
	[Header("Audio")]
	[SerializeField] private AudioConfig movementSound;

	public void Move()
	{
		movementSound.Play();
	}
}
```
