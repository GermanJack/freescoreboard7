import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GoDash } from 'react-icons/go';
import DlgPenalty from './DlgPenalty';
import DlgTime from './DlgTime';
import Table from './Table';
import DlgFeldwahl from './DlgFeldwahl';

class Strafe extends Component {
  constructor(props) {
    super(props);
    this.state = {
      selid: 0,
    };
  }

  newPenalty(team, e) {
    const t = this.teamToChar(team);
    this.props.websocketSend({
      Domain: 'KO',
      Type: 'bef',
      Command: 'newPenalty',
      Team: t,
      Player: e.playerid,
      Value1: e.penaltyid,
    });
  }

  rowClick(e) {
    this.setState({ selid: e });
  }

  delsel(team) {
    const t = this.teamToChar(team);
    this.props.websocketSend({
      Domain: 'KO',
      Type: 'bef',
      Command: 'delPenalty',
      Team: t,
      Value1: this.state.selid,
    });
    this.setState({ selid: 0 });
  }

  newTime(team, e) {
    const t = this.teamToChar(team);
    this.props.websocketSend({
      Domain: 'KO',
      Type: 'bef',
      Command: 'ManipulatePenalty',
      Team: t,
      Value1: this.state.selid,
      Value2: e,
    });
  }

  teamToChar(team) {
    let t = '';
    if (team === '1') {
      t = 'A';
    } else {
      t = 'B';
    }

    return t;
  }

  render() {
    let data = [];
    let pen = null;
    let penTime = '';
    let penPlayer = '';

    if (this.props.tabellenvariablen) {
      const variable = this.props.tabellenvariablen.find((x) => x.ID === this.props.variable);
      if (variable) {
        data = JSON.parse(variable.Wert);
      }

      if (data && this.state.selid !== 0) {
        pen = data.find((x) => x.ID === this.state.selid);
        if (pen) {
          penTime = pen.Minuten;
          penPlayer = pen.Spieler;
        }
      }
    }

    let disabled = true;
    if (data) {
      if (data.length > 0 && this.state.selid !== 0) {
        disabled = false;
      }
    }

    const TabFelder = [];
    for (let i = 0; i < this.props.anzeigetabellen.length; i += 1) {
      const o = this.props.anzeigetabellen[i];
      if (o.Tabelle === 'Strafen' && o.ausblendbar === 1) {
        TabFelder.push(o);
      }
    }

    return (
      <div>
        <div className="d-flex flex-row mt-1 mb-1">
          <div>
            {`Strafen ${this.props.teamName}`}
          </div>
          <DlgPenalty
            team={this.props.team}
            playerlist={this.props.playerlist}
            penalties={this.props.penalties}
            modalID={`Penalty${this.props.team}`}
            label1={`Strafe Mannschaft ${this.props.teamName}`}
            newPenalty={this.newPenalty.bind(this, this.props.team)}
          />
          <button
            type="button"
            className="btn btn-primary border-dark ml-1 p-0"
            onClick={this.delsel.bind(this, this.props.team)}
            disabled={disabled}
          >
            <GoDash size="1.5em" color="white" />
          </button>
          <DlgTime
            btnclassname="btn btn-primary border-dark ml-1 p-0 pl-1 pr-1"
            wert={penTime}
            modalID={`editsa${this.state.selid}`}
            displaymod="min"
            label1={`Strafzeit für ${penPlayer} ändern:`}
            toolTip={`Strafzeit für ${penPlayer} ändern:`}
            disabled={disabled}
            newTime={this.newTime.bind(this, this.props.team)}
          />
          <DlgFeldwahl
            werte={TabFelder}
            label1="Feldwahl"
            toolTip="Feldwahl"
            modalID="FeldwahlStrafen"
            // onChange={this.props.handleFenVis.bind(this)}
            websocketSend={this.props.websocketSend.bind(this)}
          />
        </div>

        <div>
          <Table
            daten={data}
            chk="selected"
            chkid={[this.state.selid]}
            cols={[]}
            rowClick={this.rowClick.bind(this)}
          />
        </div>
      </div>
    );
  }
}

Strafe.propTypes = {
  variable: PropTypes.string,
  playerlist: PropTypes.arrayOf(PropTypes.object),
  anzeigetabellen: PropTypes.arrayOf(PropTypes.object),
  penalties: PropTypes.arrayOf(PropTypes.object),
  tabellenvariablen: PropTypes.arrayOf(PropTypes.object),
  team: PropTypes.string,
  teamName: PropTypes.string,
  websocketSend: PropTypes.func,
};

Strafe.defaultProps = {
  variable: '',
  playerlist: [],
  anzeigetabellen: [],
  penalties: [],
  tabellenvariablen: [],
  team: '',
  teamName: '',
  websocketSend: () => {},
};

export default Strafe;
