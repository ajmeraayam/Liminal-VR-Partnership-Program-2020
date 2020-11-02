# Alchemy

## What is Liminal VR and What is Alchemy?

The Liminal VR platform is a neuroscience-based platform that consists of short virtual reality experiences that allow people to consciously choose how they want to feel and perform. While the platform is intended for wide variety of audience (virtual reality users), the experience selected is intended to be for specific audience. This project is to develop and test an experience categorized as “pain relief” using the Unity Game Engine within Oculus Quest in accordance with Liminal’s Psych Docs and SDK, which will be a perpetual gameplay. 

The experience is titled Alchemy: where the user is inside a hut surrounded with calming environment, they are given task to brew potions in accordance with the recipes provided within the instruction board. While there a fixed goal the user must achieve is not present, the aim of the experience is to deliver a semi linear flow to the user to allow them to distract themselves from any discomforts (particularly pain).

## Technical Framework

**Primary Technologies, Environments and Languages**

- Unity game engine – To setup the scene of the experience that involves game objects that are interactable and non-interactable. It can also generate APKs for loading the experience into a VR device.
- C# - Script the backend of the experience. Includes handlers for VR controller, handlers for animation of ingredients and pot, level handlers, score manager, timers, recipe generator, etc
- Liminal SDK – This package is used to bundle scripts, a Unity Scene and Assets into a (.limapp) file. The (.limapp) file is required for the Liminal Platform to run as it as an experience. The Liminal SDK provides its own copies of the Oculus VR & Gear VR SDKs. Provides a VR emulator for developers to test the scene on Unity.
- Oculus Quest – To test the features that are developed by the team on a VR device.
- SideQuest – To connect and load the APKs on Oculus Quest
- GitHub – Source and version control of the experience.
- Liminal Platform – Refer to the experiences that are already up and running on Liminal’s platform for their users.

**Secondary Technologies, Environments and Languages**

- FL Studio – Used for music production that will be used as background tracks in the experience.
- Entrow – Postproduction and editing of the music that is created in FL Studio.
- Visual Studio/VS Code – IDE for C# scripting. Supported by Unity game engine.
- Jira – Project management board
- MS Teams – Communication within the team, with the Product Owner and project supervisor.

## Getting Started

Refer [this](https://github.com/ajmeraayam/Liminal-VR-Partnership-Program-2020/wiki/Development-Guide) link for Development Guide and Getting Started.
Refer [this](https://github.com/ajmeraayam/Liminal-VR-Partnership-Program-2020/wiki) link for a detailed Project Report.