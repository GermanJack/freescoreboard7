import React, { Component } from 'react';
import { BsFullscreen } from 'react-icons/bs';
import './App.css';
import Options from './options/options';
import Control from './control/Control';
import Teams from './teams/Teams';
import DisplayDesigner from './displaydesigner/dispalydesigner';
import Debug from './debug/debug';
import Logon from './logon/logon';
import Info from './info/info';
import Turniere2 from './turniere/turniere2';
import DlgMessage from './StdComponents/DlgMessage';

class App extends Component {
  constructor() {
    super();
    this.state = {
      Version: 0,

      websocketopen: false,
      linkid: 0,
      loggedon: true,
      HeartBeatStatus: false,
      WebKontrols: [],
      fullscreen: false,

      picList: [],
      fontList: [],
      soundList: [],

      events: [],
      options: [],
      tabsort: [],
      penalties: [],
      PageSets: [],
      // displayPageSet: '',
      displayPages: [],
      designerPages: [],
      designerPage: '',

      allDivs: [],
      // actualURL: '',

      teamList: [],
      playerList: [], // Enthgält nur Spieler der in der Mannschaftenverwaltung gewählten mannschaft.
      playerListAll: [], // Eine Liste aller Spieler
      playerListTeam1: [],
      playerListTeam2: [],
      teamAid: '',
      teamBid: '',

      timer: [],
      timerObjects: [],
      timerevents: [],

      picVariables: [],
      textvariablen: [],
      tabellenvariablen: [],

      turniere: [],
      turnierID: null,
      runden: [],
      gruppen: [],
      spiele: [{ Spiel: '0' }],

      tableFilter: [],
      anzeigetabellen: [],

      audioObjects: [],
    };
  }

  componentDidMount() {
    this.setLink('Kontrolle');
    this.setState({ linkid: 0 });

    this.iniWebsocket();
  }

  static getQueryVariable(variable) {
    const query = window.location.search.substring(1);
    const vars = query.split('&');
    for (let i = 0; i < vars.length; i += 1) {
      const pair = vars[i].split('=');
      if (pair[0] === variable) {
        return pair[1];
      }
    }

    return (false);
  }

  setLink(linkid) {
    // const s = this.state;
    if (this.state.loggedon) {
      this.setState({ linkid });
    } else {
      this.setState({ linkid: 0 });
    }
  }

  iniWebsocket() {
    // const s = this.state;

    if (window.location.search.includes('?')) {
      const websocketHost = window.location.hostname;
      const websocketPort = this.constructor.getQueryVariable('wsp');
      const websocketProtokoll = 'FSB';
      const wsUri = `ws://${websocketHost}:${websocketPort}/${websocketProtokoll}`;
      this.connection = new WebSocket(wsUri);

      this.connection.onmessage = (evt) => {
        let j;
        try {
          j = JSON.parse(evt.data);
        } catch (e) {
          let es = '[leer]';
          if (evt.data !== '') {
            es = evt.data;
          }
          console.log(`parse Error: ${es}`); // eslint-disable-line no-console
        }

        if (j) {
          // Server-Error handling
          if (j.Domain === 'Error') {
            console.log(j.Value1); // eslint-disable-line no-console
            if (j.Value2 === '') {
              alert('Ein Fehler ist aufgetreten.\r\n(Details in der Debug Konsole.)'); // eslint-disable-line no-alert
            }
          }

          if (j.Domain === 'Message') {
            // eslint-disable-next-line no-alert
            window.alert(j.Value1);
          }

          if (j.Domain === 'loggedon') {
            if (j.Value1 === 'True') {
              this.setState({ loggedon: true });
              this.setState({ linkid: 10 });
            } else {
              this.setState({ loggedon: false });
            }
          }

          // Page Designer
          if (j.Type === 'SB') {
            if (j.Command === 'refreshDivPos' && j.PageSet === this.state.profil && j.Page === this.state.page.Name) {
              // eslint-disable-next-line react/no-access-state-in-setstate
              const o = this.state.activeObject;
              o.style.left = j.Value1;
              o.style.top = j.Value2;
              this.setState({ activeObject: o });
            }
            if (j.Command === 'refreshDivSiz' && j.PageSet === this.state.profil && j.Page === this.state.page.Name) {
              // eslint-disable-next-line react/no-access-state-in-setstate
              const o = this.state.activeObject;
              o.style.width = j.Value1;
              o.style.height = j.Value2;
              this.setState({ activeObject: o });
            }
          }

          // single Variable change
          if (j.Command === 'HeartBeatStatus') {
            this.setState({ HeartBeatStatus: j.Value1 });
          }

          if (j.Domain === 'AN' && j.Command === 'textvar') {
            const tvar = j.Property;
            const wert = j.Value1;
            // eslint-disable-next-line react/no-access-state-in-setstate
            const varlist = this.state.textvariablen;
            const ind = varlist.map((m) => m.ID).indexOf(tvar);
            varlist[ind].Wert = wert;
            this.setState({ textvariablen: varlist });
          }

          if (j.Domain === 'AN' && j.Command === 'picvar') {
            const tvar = j.Property;
            const wert = j.Value1;
            // eslint-disable-next-line react/no-access-state-in-setstate
            const varlist = this.state.picVariables;
            const ind = varlist.map((m) => m.ID).indexOf(tvar);
            varlist[ind].Wert = wert;
            this.setState({ picVariables: varlist });
          }

          if (j.Domain === 'AN' && j.Command === 'tabvar') {
            const tvar = j.Property;
            const wert = j.Value1;
            // eslint-disable-next-line react/no-access-state-in-setstate
            const varlist = this.state.tabellenvariablen;
            const ind = varlist.map((m) => m.ID).indexOf(tvar);
            varlist[ind].Wert = wert;
            this.setState({ tabellenvariablen: varlist });
          }

          if (j.Domain === 'AN' && j.Command === 'toogleAudio') {
            const KontrolleAudio = (this.state.options.filter((x) => x.Prop === 'KontrolleAudio')[0].Value === 'True');
            if (!KontrolleAudio) {
              return;
            }
            const file = `../sounds/${j.Value1}`;

            let audioObj = null;
            if (this.state.audioObjects.length > 0) {
              audioObj = this.state.audioObjects.find((x) => x.text === file);
            }
            if (audioObj === undefined || audioObj === null) {
              audioObj = new Audio(file);
              audioObj.text = file;
              // eslint-disable-next-line react/no-access-state-in-setstate
              const a = this.state.audioObjects;
              a.push(audioObj);
              this.setState({ audioObjects: a });
            }

            if (audioObj.paused) {
              audioObj.play();
            } else {
              audioObj.pause();
              audioObj.currentTime = 0;
            }
          }

          if (j.Domain === 'AN' && j.Command === 'allAudioStop') {
            if (this.state.audioObjects.length > 0) {
              for (let i = 0; i < this.state.audioObjects.length; i += 1) {
                const audioObj = this.state.audioObjects[i];
                audioObj.pause();
                audioObj.currentTime = 0;
              }
              this.setState({ audioObjects: [] });
            }
          }

          if (j.Command === 'Version') {
            this.setState({ Version: j.Value1 });
          }

          // JSON data transmit
          if (j.Type === 'JN') {
            if (j.Command === 'WebKontrols') {
              const data = JSON.parse(j.Value1);
              this.setState({ WebKontrols: data });
            }

            if (j.Command === 'TeamList') {
              // create entry for player without team assignment
              // let noTeam = {};
              // noTeam = {
              //   ID: '0',
              //   Name: '[keine Mannschaft]',
              //   Kurzname: '',
              //   Bild1: '',
              //   Bild2: '',
              // };

              const data = JSON.parse(j.Value1);
              // data.unshift(noTeam);
              this.setState({ teamList: data });
            }

            if (j.Command === 'PlayerList') {
              const team = j.Value1;
              const daten = JSON.parse(j.Value2);
              // Team A oder Team B wird von Kontrolle angefordert
              // Mannschaftspflege fordert mit TeamID an
              if (team === 'A') {
                this.setState({ playerListTeam1: daten });
              } else if (team === 'B') {
                this.setState({ playerListTeam2: daten });
              } else if (team === 'ALL') {
                this.setState({ playerListAll: daten });
              } else {
                this.setState({ playerList: daten });
              }
            }

            if (j.Command === 'TeamID') {
              const team = j.Team;
              const daten = j.Value1;
              if (team === 'A') {
                this.setState({ teamAid: daten });
              } else {
                this.setState({ teamBid: daten });
              }
            }

            if (j.Command === 'PageSets') {
              const daten = JSON.parse(j.Value1);
              this.setState({ PageSets: daten });
            }

            // PageList for active PageSet
            if (j.Command === 'PageList') {
              const daten = JSON.parse(j.Value1);
              this.setState({ displayPages: daten });
            }

            // Pages for selected Pageset in Designer
            if (j.Command === 'Pages') {
              const daten = JSON.parse(j.Value1);
              this.setState({ designerPages: daten });
            }

            if (j.Command === 'Page') {
              const daten = JSON.parse(j.Value1);
              this.setState({ designerPage: daten });
            }

            if (j.Command === 'Divs') {
              const daten = JSON.parse(j.Value1);
              this.setState({ allDivs: daten });
            }

            if (j.Command === 'Options') {
              const daten = JSON.parse(j.Value1);
              this.setState({ options: daten });
            }

            if (j.Command === 'TabellenSort') {
              const daten = JSON.parse(j.Value1);
              this.setState({ tabsort: daten });
            }

            if (j.Command === 'ListeTextVariablen') {
              const daten = JSON.parse(j.Value1);
              this.setState({ textvariablen: daten });
            }

            if (j.Command === 'ListeBildVariablen') {
              const daten = JSON.parse(j.Value1);
              this.setState({ picVariables: daten });
            }

            if (j.Command === 'ListeTabellenVariablen') {
              const daten = JSON.parse(j.Value1);
              this.setState({ tabellenvariablen: daten });
            }

            if (j.Command === 'Events') {
              const daten = JSON.parse(j.Value1);
              this.setState({ events: daten });
            }

            if (j.Command === 'Penalties') {
              const daten = JSON.parse(j.Value1);
              this.setState({ penalties: daten });
            }

            if (j.Command === 'PictureList') {
              const daten = JSON.parse(j.Value1);
              daten.unshift('[kein Bild]');
              this.setState({ picList: daten });
            }

            if (j.Command === 'AudioFileList') {
              const daten = JSON.parse(j.Value1);
              daten.unshift('[kein Ton]');
              this.setState({ soundList: daten });
            }

            if (j.Command === 'FontList') {
              const daten = JSON.parse(j.Value1);
              // this.setState({ fontFileList: daten });
              const fl = [];
              for (let i = 0; i < daten.length; i += 1) {
                const fontfile = daten[i];
                const fontname = daten[i].split('.')[0];
                fl.push({ fontfile, fontname });
              }

              this.setState({ fontList: fl });

              const sheet = window.document.styleSheets[0];
              for (let i = 0; i < fl.length; i += 1) {
                const f = `@font-face{font-family:'${fl[i].fontname}';src:url('../../../../../fonts/${daten[i]}');}`;
                sheet.insertRule(f, sheet.cssRules.length);
              }
            }

            if (j.Command === 'Timers') {
              const daten = JSON.parse(j.Value1);
              this.setState({ timer: daten });
            }

            if (j.Command === 'TimerObjects') {
              const daten = JSON.parse(j.Value1);
              this.setState({ timerObjects: daten });
            }

            if (j.Command === 'Timerevents') {
              const data = JSON.parse(j.Value1);
              this.setState({ timerevents: data });
            }

            if (j.Command === 'TableFilter') {
              const data = JSON.parse(j.Value1);
              this.setState({ tableFilter: data });
            }

            if (j.Command === 'Anzeigetabellen') {
              const data = JSON.parse(j.Value1);
              this.setState({ anzeigetabellen: data });
            }

            if (j.Command === 'TurniereKomplett') {
              const data = JSON.parse(j.Value1);
              this.setState({ turniere: data });
            }

            if (j.Command === 'turnierID') {
              const data = JSON.parse(j.Value1);
              this.setState({ turnierID: data });
            }
          }
        }
      };

      this.connection.onopen = () => {
        this.setState({ websocketopen: true });
        this.websocketSend({ Type: 'loggedon' });
        this.websocketSend({ Type: 'req', Command: 'Version' });

        this.websocketSend({ Type: 'req', Command: 'HeartBeatStatus' });
        this.websocketSend({ Type: 'req', Command: 'WebKontrols' });

        this.websocketSend({ Type: 'req', Command: 'Options' });
        this.websocketSend({ Type: 'req', Command: 'TabellenSort' });
        this.websocketSend({ Type: 'req', Command: 'Events' });
        this.websocketSend({ Type: 'req', Command: 'Penalties' });
        this.websocketSend({ Type: 'req', Command: 'Timers' });
        this.websocketSend({ Type: 'req', Command: 'TimerObjects' });

        this.websocketSend({ Type: 'req', Command: 'PageSets' });
        this.websocketSend({ Type: 'req', Command: 'PageList' });
        this.websocketSend({ Type: 'req', Command: 'WhereToGo' });

        this.websocketSend({ Type: 'req', Command: 'PictureList' });
        this.websocketSend({ Type: 'req', Command: 'AudioFileList' });
        this.websocketSend({ Type: 'req', Command: 'FontList' });

        this.websocketSend({ Type: 'req', Command: 'SVariablen' });
        this.websocketSend({ Type: 'req', Command: 'BVariablen' });
        this.websocketSend({ Type: 'req', Command: 'TVariablen' });

        this.websocketSend({ Type: 'req', Command: 'TeamList' });
        this.websocketSend({ Type: 'req', Command: 'TeamID', Team: 'A' });
        this.websocketSend({ Type: 'req', Command: 'TeamID', Team: 'B' });
        this.websocketSend({
          Type: 'req', Command: 'PlayerList', Team: this.state.teamAid, Value1: 'A',
        });
        this.websocketSend({
          Type: 'req', Command: 'PlayerList', Team: this.state.teamBid, Value1: 'B',
        });
        this.websocketSend({ Type: 'req', Command: 'PlayerList', Team: 'ALL' });

        this.websocketSend({ Type: 'req', Command: 'TableFilter' });
        this.websocketSend({ Type: 'req', Command: 'Anzeigetabellen' });

        this.websocketSend({ Type: 'req', Command: 'TurniereKomplett' });
        this.websocketSend({ Type: 'req', Command: 'turnierID' });
      };

      this.connection.onclose = (e) => {
        this.setState({ websocketopen: false });
        // eslint-disable-next-line no-console
        console.log('Serververbindung wurde getrennt. Neuer Verbindungsversuch in 1 Sekunde.', e.reason);
        setTimeout(() => {
          this.iniWebsocket();
        }, 1000);
      };

      this.connection.onerror = (err) => {
        // eslint-disable-next-line no-console
        console.error('Bei der Serververbindung ist folgender Fehler aufgetreten: ', err.message, 'Verdindiung wird geschlossen.');
        this.connection.close();
      };
    }
  }

  toggleFullScreen() {
    const s = this.state;
    const element = document.documentElement;
    if (!s.fullscreen) {
      if (element.requestFullScreen) {
        element.requestFullScreen();
      } else if (element.webkitRequestFullScreen) {
        element.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
      } else if (element.mozRequestFullScreen) {
        element.mozRequestFullScreen();
      } else if (element.msRequestFullscreen) {
        element.msRequestFullscreen();
      }
      this.setState({ fullscreen: true });
    } else {
      if (document.exitFullscreen) {
        document.exitFullscreen();
      } else if (document.msExitFullscreen) {
        document.msExitFullscreen();
      } else if (document.mozCancelFullScreen) {
        document.mozCancelFullScreen();
      } else if (document.webkitExitFullscreen) {
        document.webkitExitFullscreen();
      }
      this.setState({ fullscreen: false });
    }
  }

  websocketSend({
    Domain = 'KO',
    Type = 'bef',
    PageSet = '',
    Page = '',
    Divs = [],
    Command = '',
    Property = '',
    Team = '',
    Player = '',
    Value1 = '',
    Value2 = '',
    Value3 = '',
  } = {}) {
    if (window.location.search.includes('?')) {
      const Remotebefehl = {
        Domain,
        Type,
        PageSet,
        Page,
        Divs,
        Command,
        Property,
        Team,
        Player,
        Value1,
        Value2,
        Value3,
      };

      const json = JSON.stringify(Remotebefehl);
      this.connection.send(json);
    }
  }

  websocketSendRaw(text) {
    if (window.location.search.includes('?')) {
      this.connection.send(text);
    }
  }

  zoom(e) {
    if (navigator.userAgent === 'CEF') {
      if (e.ctrlKey || e.metaKey) {
        const direction = (e.deltaY < 0) ? 'up' : 'down';
        window.nativeHost.zoom(direction);
      }
    }
  }

  render() {
    let fp = (
      <div
        role="button"
        className="fullscreen"
        title="Vollbild"
        onClick={this.toggleFullScreen.bind(this)}
        tabIndex={-1}
      >
        <BsFullscreen />
      </div>
    );
    let ws = (
      <div
        className="websocketerror"
        title="Verbindungsfehler"
        label="Verbindungsfehler"
        onClick={this.iniWebsocket.bind(this)}
        role="button"
        tabIndex={0}
      />
    );

    if (navigator.userAgent === 'CEF') {
      // kein vollbild icon notwendig
      fp = null;
    }

    if (this.state.websocketopen) {
      ws = null;
    }

    let p = null;

    if (this.state.linkid === 0) {
      p = <Logon websocketSend={this.websocketSend.bind(this)} />;
    }

    if (this.state.linkid === 10) {
      let turnier = '';
      if (this.state.turnierID !== 0 && this.state.turniere.length > 0) {
        turnier = this.state.turniere.filter((x) => x.Kopf.ID === this.state.turnierID);
      }
      p = (
        <Control
          WebKontrols={this.state.WebKontrols}
          options={this.state.options}
          anzeige={this.state.anzeige}
          picList={this.state.picList}
          fontList={this.state.fontList}
          soundList={this.state.soundList}
          events={this.state.events}
          penalties={this.state.penalties}
          PageSets={this.state.PageSets}
          websocketSend={this.websocketSend.bind(this)}
          websocketSendRaw={this.websocketSendRaw.bind(this)}
          timer={this.state.timer}
          timerObjects={this.state.timerObjects}
          timerevents={this.state.timerevents}
          teamList={this.state.teamList}
          playerlistTeam1={this.state.playerListTeam1}
          playerlistTeam2={this.state.playerListTeam2}
          playerListAll={this.state.playerListAll}
          textvariablen={this.state.textvariablen}
          picVariables={this.state.picVariables}
          tabellenvariablen={this.state.tabellenvariablen}
          seiten={this.state.displayPages}
          turnierID={this.state.turnierID}
          runden={this.state.runden}
          gruppen={this.state.gruppen}
          spiele={this.state.spiele}
          tableFilter={this.state.tableFilter}
          anzeigetabellen={this.state.anzeigetabellen}
          turnier={turnier}
        />
      );
    }

    if (this.state.linkid === 20) {
      p = (
        <Teams
          teamList={this.state.teamList}
          playerList={this.state.playerList}
          picList={this.state.picList}
          websocketSend={this.websocketSend.bind(this)}
          websocketSendRaw={this.websocketSendRaw.bind(this)}
        />
      );
    }

    if (this.state.linkid === 26) {
      p = (
        <Turniere2
          teamList={this.state.teamList}
          turniere={this.state.turniere}
          activeTurnierID={this.state.turnierID}
          websocketSend={this.websocketSend.bind(this)}
        />
      );
    }

    if (this.state.linkid === 30) {
      p = (
        <DisplayDesigner
          PageSets={this.state.PageSets}
          pages={this.state.designerPages}
          page={this.state.designerPage}
          allDivs={this.state.allDivs}
          activeObject={this.state.activeObject}
          activeObjectID={this.state.activeObjectID}
          picList={this.state.picList}
          fontList={this.state.fontList}
          picVariables={this.state.picVariables}
          textVariables={this.state.textvariablen}
          tableVariables={this.state.tabellenvariablen}
          websocketSend={this.websocketSend.bind(this)}
          websocketSendRaw={this.websocketSendRaw.bind(this)}
        />
      );
    }

    if (this.state.linkid === 40) {
      p = (
        <Options
          options={this.state.options}
          tabsort={this.state.tabsort}
          anzeige={this.state.anzeige}
          picList={this.state.picList}
          fontList={this.state.fontList}
          soundList={this.state.soundList}
          events={this.state.events}
          penalties={this.state.penalties}
          PageSets={this.state.PageSets}
          seiten={this.state.displayPages}
          websocketSend={this.websocketSend.bind(this)}
          websocketSendRaw={this.websocketSendRaw.bind(this)}
          timer={this.state.timer}
          timerevents={this.state.timerevents}
        />
      );
    }

    if (this.state.linkid === 90) {
      p = (
        <Debug
          options={this.state.options}
          HeartBeatStatus={this.state.HeartBeatStatus}
          picVariables={this.state.picVariables}
          textvariablen={this.state.textvariablen}
          tableVariables={this.state.tabellenvariablen}
          websocketSend={this.websocketSend.bind(this)}
        />
      );
    }

    if (this.state.linkid === 99) {
      p = (
        <Info
          websocketSend={this.websocketSend.bind(this)}
          Version={this.state.Version}
        />
      );
    }

    const btnon = 'mr-1 ml-1 pl-1 pr-1 btn btn-dark';
    const btnoff = 'mr-1 ml-1 pl-1 pr-1 btn btn-outline-dark';

    return (
      <div className="App" onWheel={(e) => this.zoom(e)}>
        <div className="border">
          <div className="pb-1 bg-success text-center">
            <h4>freeScoreboard - Die kostenlose Sportanzeige!</h4>
            {ws}
            {fp}
          </div>
          <div className="nav justify-content-center">
            <button type="button" className={this.state.linkid === 10 ? btnon : btnoff} onClick={this.setLink.bind(this, 10)}>Anzeigensteuerung</button>
            <button type="button" className={this.state.linkid === 20 ? btnon : btnoff} onClick={this.setLink.bind(this, 20)}>Mannschaften</button>
            <button type="button" className={this.state.linkid === 26 ? btnon : btnoff} onClick={this.setLink.bind(this, 26)}>Turniere</button>
            <button type="button" className={this.state.linkid === 30 ? btnon : btnoff} onClick={this.setLink.bind(this, 30)}>Anzeigen-Editor</button>
            <button type="button" className={this.state.linkid === 40 ? btnon : btnoff} onClick={this.setLink.bind(this, 40)}>Einstellungen</button>
            <button type="button" className={this.state.linkid === 90 ? btnon : btnoff} onClick={this.setLink.bind(this, 90)}>Debug</button>
            <button type="button" className={this.state.linkid === 99 ? btnon : btnoff} onClick={this.setLink.bind(this, 99)}>Info</button>
          </div>

          <div>
            {p}
          </div>
        </div>
      </div>
    );
  }
}

export default App;
