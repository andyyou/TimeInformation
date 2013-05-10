# IIS Express config MINE TYPE of JSON.
1. Open command line in windows
2. Navigation to the IIS Express directory. This lives under Program Files or Program Files (x86)
3. appcmd set config /section:staticContent /+[fileExtension='json',mimeType='application/json']

OR

Open C:\Users\YourID\Documents\IIS Express\config\applicationhost.config
Modify
<mimeMap fileExtension="json" mimeType="application/json" />
