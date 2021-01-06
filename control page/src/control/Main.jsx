import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Kontrolle3S from './Kontrolle3S';
import DlgFensterwahl from '../StdComponents/DlgFensterwahl';
import FilterTabellen from '../StdComponents/FilterTabellen';

class Main extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  render() {
    return (
      <div className="mt-1">
        <ul className="nav nav-pills">
          <li className="nav-item">
            <a className="nav-link active" data-toggle="tab" data-labelid="TabSetup" href="#Kontrolle">
              <div className="row p-0">
                Kontrolle
                <DlgFensterwahl
                  werte={this.props.WebKontrols}
                  icon=""
                  iconAltText="E"
                  label1="Fensterwahl"
                  toolTip="Fensterwahl"
                  modalID="Fensterwahl"
                  // onChange={this.props.handleFenVis.bind(this)}
                  websocketSend={this.props.websocketSend.bind(this)}

                />
              </div>
            </a>
          </li>
          <div>|</div>
          <li className="nav-item">
            <a className="nav-link" data-toggle="tab" data-labelid="TabBedienung" href="#TabelleE">Ereignisse</a>
          </li>
          <div>|</div>
          <li className="nav-item">
            <a className="nav-link" data-toggle="tab" data-labelid="TabBedienung" href="#TabelleT">Tabellen</a>
          </li>
          <div>|</div>
          <li className="nav-item">
            <a className="nav-link" data-toggle="tab" data-labelid="TabBedienung" href="#TabelleP">Spielplan</a>
          </li>
          <div>|</div>
          <li className="nav-item">
            <a className="nav-link" data-toggle="tab" data-labelid="TabBedienung" href="#TabelleH">Torschütze</a>
          </li>
        </ul>

        <div className="tab-content bg-light">

          <div className="tab-pane fade show active" id="Kontrolle">
            <Kontrolle3S
              ref={(kont) => { window.Kontrolle = kont; }}
              WebKontrols={this.props.WebKontrols}
              textvariablen={this.props.textvariablen}
              picVariables={this.props.picVariables}
              tabellenvariablen={this.props.tabellenvariablen}
              teamList={this.props.teamList}
              playerlistTeam1={this.props.playerlistTeam1}
              playerlistTeam2={this.props.playerlistTeam2}
              options={this.props.options}
              websocketSend={this.props.websocketSend.bind(this)}
              timer={this.props.timer}
              timerObjects={this.props.timerObjects}
              penalties={this.props.penalties}
              anzeigetabellen={this.props.anzeigetabellen}
              turnierID={this.props.turnierID}
              turnier={this.props.turnier}
              pages={this.props.seiten}

              // onElementsChange={this.props.onElementsChange.bind(this)}
            />
          </div>

          <div className="tab-pane fade" id="TabelleE">
            <FilterTabellen
              ref={(kont) => { window.Tabelle1 = kont; }}
              textvariablen={this.props.textvariablen}
              tabellenvariablen={this.props.tabellenvariablen}
              spiele={this.props.spiele}
              events={this.props.events}
              tableFilter={this.props.tableFilter}
              anzeigetabellen={this.props.anzeigetabellen}
              tabVariable="T03"
              teamList={this.props.teamList}
              playerlistTeam1={this.props.playerlistTeam1}
              playerlistTeam2={this.props.playerlistTeam2}
              playerListAll={this.props.playerListAll}
              websocketSend={this.props.websocketSend.bind(this)}
              turnier={this.props.turnier}
            />
          </div>

          <div className="tab-pane fade" id="TabelleT">
            <FilterTabellen
              ref={(kont) => { window.Tabelle1 = kont; }}
              tabellenvariablen={this.props.tabellenvariablen}
              spiele={this.props.spiele}
              events={this.props.events}
              tableFilter={this.props.tableFilter}
              anzeigetabellen={this.props.anzeigetabellen}
              tabVariable="T01"
              websocketSend={this.props.websocketSend.bind(this)}
            />
          </div>

          <div className="tab-pane fade" id="TabelleP">
            <FilterTabellen
              ref={(kont) => { window.Tabelle1 = kont; }}
              tabellenvariablen={this.props.tabellenvariablen}
              spiele={this.props.spiele}
              events={this.props.events}
              tableFilter={this.props.tableFilter}
              anzeigetabellen={this.props.anzeigetabellen}
              tabVariable="T02"
              websocketSend={this.props.websocketSend.bind(this)}
              turnier={this.props.turnier}
            />
          </div>

          <div className="tab-pane fade" id="TabelleH">
            <FilterTabellen
              ref={(kont) => { window.Tabelle1 = kont; }}
              tabellenvariablen={this.props.tabellenvariablen}
              spiele={this.props.spiele}
              events={this.props.events}
              tableFilter={this.props.tableFilter}
              anzeigetabellen={this.props.anzeigetabellen}
              tabVariable="T06"
              websocketSend={this.props.websocketSend.bind(this)}
            />
          </div>
          {/* <div className="tab-pane fade" id="Tabelle">
            <Table ref={(kont) => { window.Tabelle1 = kont; }} />
          </div> */}

        </div>

      </div>
    );
  }
}

Main.propTypes = {
  WebKontrols: PropTypes.arrayOf(PropTypes.object).isRequired,
  textvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  picVariables: PropTypes.arrayOf(PropTypes.object).isRequired,
  tabellenvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  tableFilter: PropTypes.arrayOf(PropTypes.object).isRequired,
  anzeigetabellen: PropTypes.arrayOf(PropTypes.object).isRequired,
  teamList: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam1: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam2: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerListAll: PropTypes.arrayOf(PropTypes.object).isRequired,
  penalties: PropTypes.arrayOf(PropTypes.object).isRequired,
  options: PropTypes.arrayOf(PropTypes.object).isRequired,
  timer: PropTypes.arrayOf(PropTypes.object).isRequired,
  timerObjects: PropTypes.arrayOf(PropTypes.object).isRequired,
  spiele: PropTypes.arrayOf(PropTypes.object).isRequired,
  events: PropTypes.arrayOf(PropTypes.object).isRequired,
  turnierID: PropTypes.string.isRequired,
  turnier: PropTypes.string.isRequired,
  websocketSend: PropTypes.func.isRequired,
  seiten: PropTypes.arrayOf(PropTypes.object).isRequired,
};

export default Main;
