import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Main from './Main';

class Control extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  // pageChange(e) {
  //   // this.setState({ displayPageName: e });
  // }

  render() {
    let tvar = '';
    let tname = '';
    if (this.props.textvariablen.length > 0) {
      tvar = this.props.textvariablen.filter((x) => x.ID === 'S14');
      tname = tvar[0].Wert;
    }
    let cN = 'text-center bg-fsc text-light p-1 mb-1';
    if (this.props.turnierID !== 0) {
      cN = 'text-center bg-warning text-body p-1 mb-1';
    }

    return (
      <div className="border border-dark">
        <div className={cN}>
          {`Anzeigensteuereung - Turnier: ${tname}`}
        </div>

        <div className="mt-1 mb-1 mr-1 bg-tabs">
          <Main
            WebKontrols={this.props.WebKontrols}

            textvariablen={this.props.textvariablen}
            picVariables={this.props.picVariables}
            tabellenvariablen={this.props.tabellenvariablen}
            websocketSend={this.props.websocketSend.bind(this)}
            tableFilter={this.props.tableFilter}
            anzeigetabellen={this.props.anzeigetabellen}

            // onElementsChange={this.handleElementsChange.bind(this)}
            // handleFenVis={this.handleFenVis.bind(this)}

            teamList={this.props.teamList}
            playerlistTeam1={this.props.playerlistTeam1}
            playerlistTeam2={this.props.playerlistTeam2}
            playerListAll={this.props.playerListAll}

            options={this.props.options}
            timer={this.props.timer}
            timerObjects={this.props.timerObjects}

            spiele={this.props.spiele.map((x) => x.Spiel)}
            events={this.props.events} // .map((x) => x.Nummer)}
            runden={this.props.runden.map((x) => x.Runde)}
            gruppen={this.props.gruppen.map((x) => x.Gruppe)}

            penalties={this.props.penalties}

            turnierID={this.props.turnierID}
            turnier={this.props.turnier}

            seiten={this.props.seiten}
          />
        </div>

        {/* <Seitenwahl
          pages={this.props.seitenNamen}
          websocketSend={this.props.websocketSend.bind(this)}
        /> */}

        {/* <Seitenwahl2
          pages={this.props.seiten}
          websocketSend={this.props.websocketSend.bind(this)}
        /> */}

        {/* <div className="row">
          <div className="col p-0 justify-content-left">
            <Tonwahl
              websocketSend={this.props.websocketSend.bind(this)}
            />
          </div>
          <div className="col p-0 justify-content-left">
            <Bildwahl
              websocketSend={this.props.websocketSend.bind(this)}
            />
          </div>
        </div> */}
      </div>
    );
  }
}

Control.propTypes = {
  WebKontrols: PropTypes.arrayOf(PropTypes.object).isRequired,
  textvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  picVariables: PropTypes.arrayOf(PropTypes.object).isRequired,
  tabellenvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  tableFilter: PropTypes.arrayOf(PropTypes.object).isRequired,
  anzeigetabellen: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam1: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam2: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerListAll: PropTypes.arrayOf(PropTypes.object).isRequired,
  seiten: PropTypes.arrayOf(PropTypes.object).isRequired,
  penalties: PropTypes.arrayOf(PropTypes.object).isRequired,
  teamList: PropTypes.arrayOf(PropTypes.object).isRequired,
  options: PropTypes.arrayOf(PropTypes.object).isRequired,
  timer: PropTypes.arrayOf(PropTypes.object).isRequired,
  timerObjects: PropTypes.arrayOf(PropTypes.object).isRequired,
  spiele: PropTypes.arrayOf(PropTypes.object).isRequired,
  events: PropTypes.arrayOf(PropTypes.object).isRequired,
  runden: PropTypes.arrayOf(PropTypes.object).isRequired,
  gruppen: PropTypes.arrayOf(PropTypes.object).isRequired,
  turnierID: PropTypes.string.isRequired,
  turnier: PropTypes.string.isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default Control;
