import React, { Component }  from 'react';
import './App.css';
import Ticker from 'react-ticker'
import { Rnd } from 'react-rnd';
import Table from './Table';

class App extends Component {
  constructor() {
    super();
    this.state = {
      websocketopen: false,

      DisplayObjects: [],

      picVariables: [],
      textvariablen: [],
      tabellenvariablen: [],

      pageset: "",
      page: "",
      divid: "",

      objpage: "",

      options: [],
      audioObjects: [],
    };
  }
  componentDidMount() {
    const s = this.getQueryVariable("pageset");
    const p = this.getQueryVariable("page");
    const o = this.getQueryVariable("divid");
    this.setState({pageset: s});
    this.setState({page: p});
    this.setState({divid: o});
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
  
  iniWebsocket() {
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
          // single Variable change
          if (j.Domain === 'AN' && j.Command === 'textvar') {
            const tvar = j.Property;
            const wert = j.Value1;
            const varlist = this.state.textvariablen; // eslint-disable-line react/no-access-state-in-setstate
            const ind = varlist.map((m) => m.ID).indexOf(tvar);
            varlist[ind].Wert = wert;
            this.setState({ textvariablen: varlist });
          }

          if (j.Domain === 'AN' && j.Command === 'picvar') {
            const tvar = j.Property;
            const wert = j.Value1;
            const varlist = this.state.picVariables; // eslint-disable-line react/no-access-state-in-setstate
            const ind = varlist.map((m) => m.ID).indexOf(tvar);
            varlist[ind].Wert = wert;
            this.setState({ picVariables: varlist });
          }

          if (j.Domain === 'AN' && j.Command === 'tabvar') {
            const tvar = j.Property;
            const wert = j.Value1;
            const varlist = this.state.tabellenvariablen; // eslint-disable-line react/no-access-state-in-setstate
            const ind = varlist.map((m) => m.ID).indexOf(tvar);
            varlist[ind].Wert = wert;
            this.setState({ tabellenvariablen: varlist });
          }

          if (j.Domain === 'AN' && j.Command === 'DivVisibility') {
            const tvar = j.Property;
            const visibility = j.Value1;
            const divlist = this.state.DisplayObjects; // eslint-disable-line react/no-access-state-in-setstate
            
            for (let i = 0; i < divlist.length; i +=1) {
                if (divlist[i].textid === tvar) {
                  let s = JSON.parse(divlist[i].style);
                  s['visibility'] = visibility;
                  divlist[i].style = JSON.stringify(s);
                }
            }
            
            this.setState({ DisplayObjects: divlist });
          }

          if (j.Domain === 'AN' && j.Command === 'toogleAudio') {
            const Vorschau = this.inIframe();
            if (Vorschau) {
              return;
            }

            const AnzeigeAudio = (this.state.options.filter((x) => x.Prop === 'AnzeigeAudio')[0].Value === 'True');
            if (!AnzeigeAudio) {
              return;
            }
            const file = `../sounds/${j.Value1}`;

            let audioObj = null;
            if (this.state.audioObjects.length > 0){
              audioObj = this.state.audioObjects.find((x) => x.text === file);

            }
            if (audioObj === undefined || audioObj === null) {
              audioObj = new Audio(file);
              audioObj.text = file;
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

          // if (j.Command === 'gotopage') {
          //   this.setState({ actualURL: j.Value1 });
          // }

          // JSON data transmit
          if (j.Type === 'JN') {

            if (j.Command === 'Reload') {
              // eslint-disable-next-line no-restricted-globals
              location.reload();
            }

            if (j.Command === 'DivsActivePage') {
              const daten = JSON.parse(j.Value1);
              this.setState({ DisplayObjects: daten });
            }

            if (j.Command === 'Divs' && this.state.pageset) {
              const daten = JSON.parse(j.Value1);
              this.setState({ DisplayObjects: daten });
            }

            if (j.Command === 'ActivePage') {
              const daten = JSON.parse(j.Value1);
              this.setState({ objpage: daten });
            }

            if (j.Command === 'Page') {
              const daten = JSON.parse(j.Value1);
              this.setState({ objpage: daten });
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

            if (j.Command === 'FontList') {
              const daten = JSON.parse(j.Value1);
              // this.setState({ fontFileList: daten });
              const fl = [];
              for (let i = 0; i < daten.length; i += 1) {
                const fontfile = daten[i];
                const fontname = daten[i].split('.')[0];
                fl.push({ fontfile, fontname });
              }

              const sheet = window.document.styleSheets[0];
              for (let i = 0; i < fl.length; i += 1) {
                const f = `@font-face{font-family:'${fl[i].fontname}';src:url('../../../../../fonts/${daten[i]}');}`;
                sheet.insertRule(f, sheet.cssRules.length);
              }
            }

            if (j.Command === 'Options') {
              const daten = JSON.parse(j.Value1);
              this.setState({ options: daten });
            }
          }
        }
      };

      this.connection.onopen = () => {
        this.setState({ websocketopen: true });

        this.websocketSend({ Type: 'req', Command: 'FontList' });

        this.websocketSend({ Type: 'req', Command: 'SVariablen' });
        this.websocketSend({ Type: 'req', Command: 'BVariablen' });
        this.websocketSend({ Type: 'req', Command: 'TVariablen' });
        this.websocketSend({ Type: 'req', Command: 'Options' });

        if (!this.state.page)
        {
          this.websocketSend({ Type: 'req', Command: 'ActivePage' });
        } else {
          this.websocketSend({
            Type: 'req',
            Command: 'Page',
            PageSet: this.state.pageset,
            Page: this.state.page,
           });
        }

        if (!this.state.page) {
          this.websocketSend({ Type: 'req', Command: 'DivsActivePage' });
        } else {
          this.websocketSend({
            Type: 'req',
            Command: 'Divs',
            PageSet: this.state.pageset,
            Page: this.state.page,
          });
        }
      };

      this.connection.onclose = (e) => {
        this.setState({ websocketopen: false });
        console.log('Serververbindung wurde getrennt. Neuer Verbindungsversuch in 1 Sekunde.', e.reason);
        setTimeout(() => {
          this.componentDidMount();
        }, 1000);
      };

      this.connection.onerror = (err) => {
        console.error('Bei der Serververbindung ist folgender Fehler aufgetreten: ', err.message, 'Verdindiung wird geschlossen.');
        this.connection.close();
      };
    }
  }

  setBodyStyle(style) {
    // document.body.style.backgroundColor = style['background-color'];
    // document.body.style.backgroundImage = style['background-image'];
    const entries = Object.entries(style);
    for (let i = 0; i < entries.length; i += 1) {
      document.body.style[entries[i][0]] = entries[i][1];
    }
  }

  getQueryVariable(variable)
  {
    const query = window.location.search.substring(1);
    // console.log(query)//"app=article&act=news_content&aid=160990"
    const vars = query.split("&");
    // console.log(vars) //[ 'app=article', 'act=news_content', 'aid=160990' ]
    for (let i = 0; i < vars.length; i += 1) {
      const pair = vars[i].split("=");
      // console.log(pair)//[ 'app', 'article' ][ 'act', 'news_content' ][ 'aid', '160990' ] 
      if (pair[0] === variable) {
        return pair[1];}
      }
      return(false);
  }

  onResize(){

  }

  pxToVh(pixel) {
    const hinvh = parseFloat(pixel) * 100 / parseFloat(document.documentElement.clientHeight);
    return hinvh.toFixed(2);
  }

  pxToVw(pixel) {
    const winvw = parseFloat(pixel) * 100 / parseFloat(document.documentElement.clientWidth);
    return winvw.toFixed(2);
  }

  vhToPx(vh) {
    const winpx = (parseFloat(document.documentElement.clientHeight) / 100) * parseFloat(vh);
    return Math.round(winpx);
  }

  vwToPx(vw) {
    const winpx = (parseFloat(document.documentElement.clientWidth) / 100) * parseFloat(vw);
    return Math.round(winpx);
  }

  setDivPos(top, left) {
    if (!this.state.divid || this.state.DisplayObjects.length === 0) {
        return;
    }

    const d = document.querySelectorAll("[data-oid='" + window.divid + "']")[0];
    d.style.top = top;
    d.style.left = left;
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

  inIframe () {
    try {
        return window.self !== window.top;
    } catch (e) {
        return true;
    }
  }

  render() {
    // set body style
    if (this.state.objpage) {
      let bstyl = JSON.parse(this.state.objpage.Style);
      this.setBodyStyle(bstyl);
    }

    // red point for Websockect connection status
    let ws = <div className="websocketerror" title="Verbindungsfehler" label="Verbindungsfehler" onClick={this.iniWebsocket.bind(this)} role="button" tabIndex={0} />;
    if (this.state.websocketopen) {
      ws = null;
    }

    // define marking div
    let markdiv = null;
    if (this.state.divid && this.state.DisplayObjects.length > 0) {
      // copy array
      const dol = this.state.DisplayObjects
      // get index of div in array
      const divindex =  dol.findIndex((x) => x.ID.toString() === this.state.divid);
      // get div with id
      const d = dol[divindex];
      if (d) {
        // parse style
        const s = JSON.parse(d.style);
        const markstyle = {
          // top: this.vhToPx(s.top) + 'px',
          // left: this.vwToPx(s.left) + 'px',
          // height: s.height,
          // width: s.width,
          boxSizing: "border-box",
          position: "absolute",
          borderColor: this.state.objpage.MarkColor,
          borderStyle: "dashed",
          borderWidth: "2px",
          zIndex: "990",
          display: "flex",
        };
        
        const grid = this.state.objpage.Grid;

        markdiv = (
        <Rnd 
          id="mark"
          style={markstyle}
          dragGrid={[grid, grid]}
          resizeGrid={[grid, grid]}
          size={{ width: s.width, height: s.height }}
          position={{ x: this.vwToPx(s.left), y: this.vhToPx(s.top) }}
          enableResizing={{ top:false, right:true, bottom:true, left:false, topRight:false, bottomRight:true, bottomLeft:false, topLeft:false }}

          onDragStart={(e, ref) => {
          }}

          onDrag={(e, ref) => {
            const nl = this.pxToVw(ref.x);
            s.left = nl + "vw";
            const nt = this.pxToVh(ref.y);
            s.top = nt + "vh";
            d.style = JSON.stringify(s);
            dol[divindex] = d;
            this.setState({DisplayObjects: dol});
          }}

          onDragStop={(e, ref) => {
            this.websocketSend({
                Domain: "DD",
                Type: "bef",
                Command: "setDivStyleString",
                PageSet: this.state.pageset,
                Page: this.state.page,
                Divs: [this.state.divid],
                Property: "style",
                Value1: d.style,
            });
          }}

          onResizeStart={(e, direction, ref) => {
            if (direction === 'right' || direction === 'bottomRight') {
              this.setState({startwidth: ref.offsetWidth});
            }
            if (direction === 'bottom' || direction === 'bottomRight') {
              this.setState({startheight: ref.offsetHeight});
            }
          }}

          onResize={(e, direction, ref, delta, position) => {
            if ((direction === "right" || direction === 'bottomRight') && delta.width !== 0) {
              const nw = this.pxToVw(this.state.startwidth + delta.width);
              s.width = nw + "vw";
            }
            if ((direction === "bottom" || direction === 'bottomRight') && delta.height !== 0) {
              const nh = this.pxToVh(this.state.startheight + delta.height);
              s.height = nh + "vh";
            }
            d.style = JSON.stringify(s);
            dol[divindex] = d;
            this.setState({DisplayObjects: dol});
          }}
          
          onResizeStop={(e, direction, ref, delta, position) => {
            this.websocketSend({
                Domain: "DD",
                Type: "bef",
                Command: "setDivStyleString",
                PageSet: this.state.pageset,
                Page: this.state.page,
                Divs: [this.state.divid],
                Property: "style",
                Value1: d.style,
            });
          }}
        />
        );
      }
    }

    // calc all Divs
    const content = this.state.DisplayObjects.map((d) => {
      const s = JSON.parse(d.style);
      
      let table = '';
      if (d.tableid !== "T00" && this.state.tabellenvariablen.length > 0) {
        const js = (this.state.tabellenvariablen.find((y) => y.ID === d.tableid)).Wert;
        table = JSON.parse(js);
      }

      let text = d.innerText;
      if (d.textid !== "S00" && this.state.textvariablen.length > 0) {
        text = (this.state.textvariablen.find((y) => y.ID === d.textid)).Wert;
      }

      if (d.bgid !== "B00" && this.state.picVariables.length > 0) {
        const pic = (this.state.picVariables.find((y) => y.ID === d.bgid)).Wert;
        s['background-image'] = `url('../../pictures/${pic}')`;
      }

      if (d.tableid !== 'T00') { 
        s['overflow'] = 'auto';

        // Einheit der Schriftgrösse von vh auf vw drehen
        const f = parseFloat(s['font-size']);
        s['font-size'] = `${f}vw`;

        return (
          <div
            style={s}
            id={d.ID}
            data-textid={d.textid}
            data-bgid={d.bgid}
            data-tableid={d.tableid}
          >
            <Table
              daten={table}
              tablestyle={d.TableStyle}
            />
          </div>
        )
      } else if (d.Speed === 0) {
        return (
          <div
            style={s}
            id={d.ID}
            data-textid={d.textid}
            data-bgid={d.bgid}
            data-tableid={d.tableid}
          >
            {text}
          </div>
        )
      } else {
        return (
          <div
            style={s}
            id={d.ID}
            data-textid={d.textid}
            data-bgid={d.bgid}
            data-tableid={d.tableid}
          >
            <Ticker
              direction="toLeft"
              offset={0}
              speed={d.Speed}
              move={true}
              mode="await"
            >
              {({ index }) => (
                <>
                  {text}
                </>
              )}
            </Ticker>
          </div>
        )
      }
    })
  
    return (
      <div className="App">
        {ws}
        {content}
        {markdiv}

      </div>
    );
  }
}

export default App;
