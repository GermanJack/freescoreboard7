# freescoreboard

A software to utilize your PC as a scoreboard.

FreeScoreBoard7 is splittet into several components:
- A http / websocket server written c#
- The scorboard itself written in React.js
- the control user interface written in React.js
- The SQLite Datebase

Based on the Server Client concept, the scoreboard and the control user interface is running on each device which has an browser and is connected to the Network.
The server does need a Windows PC.
The server is using two special Windows APIs for a very exact timer and to play audio files without media player.
Therfoer the server is only running on Windows. Sorry to all Linux and Mac users.
