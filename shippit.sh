dotnet publish ./src/KeySeekerOfSplorr/KeySeekerOfSplorr.vbproj -o ./pub-linux -c Release --sc -r linux-x64
dotnet publish ./src/KeySeekerOfSplorr/KeySeekerOfSplorr.vbproj -o ./pub-windows -c Release --sc -r win-x64
dotnet publish ./src/KeySeekerOfSplorr/KeySeekerOfSplorr.vbproj -o ./pub-mac -c Release --sc -r osx-x64
butler push pub-windows thegrumpygamedev/key-seeker-of-splorr:windows
butler push pub-linux thegrumpygamedev/key-seeker-of-splorr:linux
butler push pub-mac thegrumpygamedev/key-seeker-of-splorr:mac
git add -A
git commit -m "shipped it!"