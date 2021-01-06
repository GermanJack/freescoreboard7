# freeScoreBoard

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

The technical idae is a application which has a server based state.
When the state on the sever is changeing, the controler page as well as the scorboard display page is re-rendered.
Each click on a control on the controler page is sending a command to the server.
The server is changing the state and is pushig it to all connected clients.
So, each client is doing a re-rendering.
The communication between client and server is realized via websocked.

To support also setups with local connected LED boards, it is necesary to define a location and size of the dispaly window.
This is difficult do realize with a browser.
Therefore I implemented chomium (cef-sharp) with a local form.

Dependencies:
server:
- cef-sharp
- sqLite
- EntietyFramework
- FastMember

display page:
- react-interactjs
- react-ticker

control page:
- bootstrap
- graphviz-react
- jquery
- qrcode.react
- react-color
- react-datetime
- react-icons
- react-to-print