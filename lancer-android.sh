#!/bin/zsh
set -euo pipefail
cd "${0:A:h}"
export ANDROID_HOME="${ANDROID_HOME:-$HOME/Library/Android/sdk}"
export JAVA_HOME="${JAVA_HOME:-$(/usr/libexec/java_home -v 21)}"
adb="$ANDROID_HOME/platform-tools/adb"
emulator="$ANDROID_HOME/emulator/emulator"
avd="Atelier6_Pixel10_API37"
device="emulator-5554"
case "${1:-final}" in
  final) project="CalculateurAge/CalculateurAge.csproj" ;;
  a) project="Etapes/PhaseA-code-behind/CalculateurAge/CalculateurAge.csproj" ;;
  b) project="Etapes/PhaseB-navigation/CalculateurAge/CalculateurAge.csproj" ;;
  c) project="Etapes/PhaseC-mvvm/CalculateurAge/CalculateurAge.csproj" ;;
  *) print -u2 "Usage : ./lancer-android.sh [final|a|b|c]"; exit 2 ;;
esac
if "$adb" -s "$device" get-state >/dev/null 2>&1; then
  active=$("$adb" -s "$device" emu avd name | tr -d '\r')
  if [[ "$active" != "$avd"$'\nOK' && "$active" != "$avd" ]]; then
    print -u2 "Le port 5554 est utilisé par un autre appareil : $active"
    print -u2 "Fermez cet appareil avant de lancer le Pixel 10."
    exit 1
  fi
else
  "$emulator" -avd "$avd" -port 5554 -gpu auto >/dev/null 2>&1 &
fi
for ((attempt=0; attempt<120; attempt++)); do
  if [[ $("$adb" -s "$device" shell getprop sys.boot_completed 2>/dev/null) == 1 ]]; then
    break
  fi
  sleep 1
done
if [[ $("$adb" -s "$device" shell getprop sys.boot_completed) != 1 ]]; then
  print -u2 "Android n'a pas terminé son démarrage en 120 secondes."
  exit 1
fi
dotnet build "$project" -r android-arm64 -p:TreatWarningsAsErrors=true -v minimal
apk="${project:h}/bin/Debug/net10.0-android/android-arm64/com.atelier6.calculateurage-Signed.apk"
installed=$("$adb" -s "$device" shell pm list packages com.atelier6.calculateurage | tr -d '\r')
if [[ "$installed" == package:com.atelier6.calculateurage ]]; then
  "$adb" -s "$device" shell am force-stop com.atelier6.calculateurage
fi
"$adb" -s "$device" install -r "$apk"
"$adb" -s "$device" shell am force-stop com.atelier6.calculateurage
activity=$("$adb" -s "$device" shell cmd package resolve-activity --brief -c android.intent.category.LAUNCHER com.atelier6.calculateurage | tail -n 1 | tr -d '\r')
if [[ "$activity" != com.atelier6.calculateurage/* ]]; then
  print -u2 "Activité Android introuvable : $activity"
  exit 1
fi
"$adb" -s "$device" shell am start -W -n "$activity"
