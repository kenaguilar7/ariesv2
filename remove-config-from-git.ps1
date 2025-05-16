# Remove configuration files from Git tracking while keeping them locally
git rm --cached **/*.config
git rm --cached **/appsettings*.json
git rm --cached **/launchSettings.json
git rm --cached **/*.Development.json
git rm --cached **/*.Production.json
git rm --cached **/*.Staging.json
git rm --cached **/*.Local.json
git rm --cached **/*.pubxml
git rm --cached **/*.pubxml.user
git rm --cached **/*.pfx
git rm --cached **/*.key
git rm --cached **/*.pem
git rm --cached **/*.cert
git rm --cached **/*.crt
git rm --cached **/*.cer
git rm --cached **/*.p12
git rm --cached **/*.keystore

# Stage the .gitignore changes
git add .gitignore

# Create a commit for the changes
git commit -m "Remove configuration files from Git tracking while keeping them locally" 