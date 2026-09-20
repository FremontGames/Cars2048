Unity Get Started
=================

Content:
1. [Require](#require)
2. [Install](#install)
3. [Usage](#usage)
4. [Release](#release)
1. [Resources](#resources)

----

## Require

UnityHub:
- install Unity 2022.3.12f1 LTS
    - android support 13
    - ios support 
    - visual studio 2022

Windows 10:
- install Android USB driver
    - download [Get Samsung OEM drivers](https://developer.samsung.com/galaxy/others/android-usb-driver-for-windows)
    - install (disconnect the phone!!!)
    - restart
- install JDK 8 LTS x64
    - [Get Eclipse Temurin™ Latest Releases](https://adoptium.net/download/)
    - set JAVA_HOME = `C:\Program Files\Eclipse Adoptium\jdk-8.0.392.8-hotspot\`
- set SDK env var
    - set ANDROID_SDK_ROOT = `C:\Program Files\Unity\Hub\Editor\2022.3.12f1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\`
- install ADB
    - add adb to path `%ANDROID_SDK_ROOT%\platform-tools\`
- accept license
    - `cd /d "%ANDROID_SDK_ROOT%/tools/bin"`
    - `.\sdkmanager.bat --licenses`

Phone:
- Enable Developer options
    - `Settings > About phone > Software information > Build number`
    - Tap the Build Number option seven times until you see the message You are now a developer!
- Enable USB debugging on your device
    - `Settings > System > Advanced > Developer Options > USB debugging`

----

## Install

Unity Project:
- install legacy packages
    - UI -0.0: 
    click `Unity Editor: Window Menu > Package Manager > uGUI > install`
- install googleads package
    - get [GoogleMobileAds-v8.5.3.unitypackage](https://github.com/googleads/googleads-mobile-unity/releases/tag/v8.6.0)
    - click `Unity Editor: Asset Menu > Import Package > Custom Package`
    - click `Unity Editor: Asset Menu > Google > Settings`
        - `Android`: copy AdMobAppID from [./user.txt](./user.txt)

Update Admob plugin:
- delete 

----

## Usage

### Dev on Phone

- configure phone
    - Enable the USB Debugging option under Settings > Developer options.
    - For Android 4.2 and newer, Developer options is hidden by default; use the following steps:
        - On the device, go to Settings > About <device>.
        - Tap the Build number seven times to make Settings > Developer options available.
        - Then enable the USB Debugging option.
- connect phone
    - allow USB debuging
- Unity Project:
    - go to `Unity Editor: File Menu > Build Settings > Android`
        - check dev: `dev build`
    - sign app `Publishing Settings`
        - copy password from [./user.txt](./user.txt)
        - type `Project Keystore > Password`
        - type `Project Key > Password`
    - go to `Unity Editor: File Menu > Build Profiles > Android`
    - run app
        - select `Run Device`: "Samsung SM..."
        - click `Build and Run`

Debug
````bash
cd C:\Program Files\Unity\Hub\Editor\2019.4.12f1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools
adb.exe logcat -s Unity PackageManager dalvikvm DEBUG
````

## Release

- go to `Unity Editor: Edit Menu > Project Settings > Player`
    - Version: `1.13.1` to `1.14.0`
- upgrade app version `Unity Editor: Edit Menu > Project Settings > Player >  Other settings > identification`
    - Bundle version code: `3000643` to `300064`
    - minimum API Level: `Android 13`
    - target API Level: `Auto Highest`
- go to `Unity Editor: Edit Menu > Project Settings > Player > Publishing Settings`
    - copy password from [./user.txt](./user.txt)
    - type `Project Keystore > Password`
    - type `Project Key > Password`
- Android App Bundle:
    - To configure an application to be an AAB:
    - Open the Build Profiles window (menu: File > Build Profiles).
    - From the list of platforms in the Platforms panel, select Android.
    - Select Player Settings for Android.
    - In the Publishing Settings section, enable Split Application Binary.
    - On the Build Profiles window, under Platform Settings section, enable Build App Bundle (Google Play)
- go to `Unity Editor: File Menu > Build Settings > Android`
    - click `Build`
    - create "Build" folder
    - type `com.atalantoo.game20483dcars-1.14.0-300064.aab`
    - click `Save`

release note:
```xml
<en-US>
Android 13
</en-US>
<de-DE>
Android 13
</de-DE>
<es-ES>
Android 13
</es-ES>
<fr-FR>
Android 13
</fr-FR>
<hi-IN>
Android 13
</hi-IN>
<it-IT>
Android 13
</it-IT>
<ja-JP>
Android 13
</ja-JP>
<ko-KR>
Android 13
</ko-KR>
<pt-BR>
Android 13
</pt-BR>
<ru-RU>
Android 13
</ru-RU>
<zh-TW>
Android 13
</zh-TW>
```

## Troubleshoot

### "Error building Player: BuildMethodException: [GoogleMobileAds] Android Google Mobile Ads app ID is empty. Please enter a valid app ID to run ads properly."

Assets-> GoogleMob Ads-> Setting & add app id below

## Resources

- [Debugging on an Android device](https://docs.unity3d.com/Manual/android-debugging-on-an-android-device.html)
    - [Install OEM USB drivers](https://developer.android.com/studio/run/oem-usb)
    - [Get OEM drivers](https://developer.android.com/studio/run/oem-usb#Drivers)
    - [Configure on-device developer options](https://developer.android.com/studio/debug/dev-options)
    - [Set up a device for development](https://developer.android.com/studio/run/device#setting-up)
