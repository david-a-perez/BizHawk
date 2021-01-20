#!/bin/sh
set -e
cd "$(dirname "$0")/.."
rm -fr "packaged_output" && mkdir -p "packaged_output"
find "output" -type f \( -wholename "output/EmuHawk.exe" -o -wholename "output/DiscoHawk.exe" -o -wholename "output/*.config" -o -wholename "output/defctrl.json" -o -wholename "output/EmuHawkMono.sh" -o -wholename "output/dll/*" -o -wholename "output/Shaders/*" -o -wholename "output/gamedb/*" -o -wholename "output/Tools/*" -o -wholename "output/NES/Palettes/*" -o -wholename "output/Lua/*" -o -wholename "output/Gameboy/Palettes/*" \) -not -name "*.pdb" -not -name "*.lib" -not -name "*.pgd" -not -name "*.ipdb" -not -name "*.iobj" -not -name "*.exp" -not -wholename "output/dll/libsneshawk-64*.exe" -not -name "*.ilk" -not -wholename "output/dll/gpgx.elf" -not -wholename "output/dll/miniclient.*" -exec install -D -m644 -T "{}" "packaged_{}" \;
if [ "$1" = "windows-x64" ]; then rm "packaged_output/EmuHawkMono.sh"; fi
find "packaged_output/dll" -type f -name "*.xml" -exec rm "{}" \;
mkdir "packaged_output/Firmware"
cd "packaged_output/gamedb" && 7za a -t7z -mx9 "NesCarts.7z" "NesCarts.xml" && rm "NesCarts.xml" && cd "../.."
find "packaged_output" -type f -name "*.sh" -exec chmod +x {} \; # installed with -m644 but needs to be 755
