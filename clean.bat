@echo off
echo RUNNING SCRIPT DETELE DEBUG FILE
pause

rmdir /q /s  .\SocialNetworkBE\obj
rmdir /q /s  .\SocialNetworkBE\bin
echo obj, bin in SocialNetworkBE folder clear successfully!
rmdir /q /s  .\Testing\obj
rmdir /q /s  .\Testing\bin
echo obj, bin in SocialNetworkBE folder clear successfully!
pause