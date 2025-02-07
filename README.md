# LabCam

A physical camera that saves what it captures to a file.

## Examples

![BONELAB_DarkMaze_2024-02-14_21-16-19](https://cdn.weatherelectric.xyz/github/labcam/BONELAB_DarkMaze_2024-02-14_21-16-19.png)

![BONELAB_VHSStore_2024-02-11_00-30-30](https://cdn.weatherelectric.xyz/github/labcam/BONELAB_VHSStore_2024-02-11_00-30-30.png)

![BONELAB_VHSStore_2024-02-09_23-46-44](https://cdn.weatherelectric.xyz/github/labcam/BONELAB_VHSStore_2024-02-09_23-46-44.png)


## Installation

1. Install the dependencies:
> [BoneLib](https://bonelab.thunderstore.io/package/gnonme/BoneLib/)
> 
> [MelonLoader 0.6.6](https://bonelab.thunderstore.io/package/LavaGang/MelonLoader/)
2. Drag LabCam.dll into the Mods folder.
3. Subscribe to [the SDK mod](https://mod.io/g/bonelab/m/labcam)

## Usage

Press the menu button with your right hand to take a picture.

Press the trigger with your right hand to toggle the flash.

Pictures are saved to `BONELAB\UserData\Weather Electric\LabCam`

## Fusion Support

The mod does support Fusion.

However, only one camera can be spawned at a time.

Only one camera at a time is for a few reasons:
1. I can't be bothered to make new render textures for each camera that is spawned, plus that'd be a memory management nightmare.
2. I can't be bothered to figure out how to make it so that the camera only saves pictures to whoever took it's folder, so instead there's only one camera and it gets saved to the host's folder.

## Configuration

> [LabCam]
> 
> #The quality of the images taken. Low = 480p, Medium = 720p, High = 1080p
> 
> ImageQuality = "High"

"High" and "Medium" save as .png, "Low" saves as .jpg.
