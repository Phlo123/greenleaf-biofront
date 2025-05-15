@echo off
cd /d F:\Unity\GreenLeaf - Biofront

echo Pulling latest changes...
git pull

echo Adding changes...
git add .

echo Committing...
set /p commitMsg=Enter commit message: 
git commit -m "%commitMsg%"

echo Pushing to GitHub...
git push

echo Done!
pause
