import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { IoMdArrowDropdown } from 'react-icons/io';
import DlgTable from '../../StdComponents/DlgTable';

class Spieler extends Component {
  getTeamName(p) {
    const tl = this.props.teamList;
    let t = '';
    try {
      const team = tl.find((a) => a.ID === p);
      t = team.Name;
    } catch (err) {
      t = '';
    }

    return t;
  }

  getVarWert(v) {
    const i = this.props.textvariablen.map((t) => t.ID).indexOf(v);
    return i !== -1 ? this.props.textvariablen[i].Wert : '0';
  }

  handlePlayerSelection(e) {
    this.props.websocketSend({
      Domain: 'KO',
      Type: 'bef',
      Command: 'SetPlayer',
      Value1: e.wahl,
    });
  }

  render() {
    const pl1 = this.props.playerlistTeam1;
    const pl2 = this.props.playerlistTeam2;
    const plc = pl1.concat(pl2);

    const ml = [];
    if (plc) {
      for (let i = 0; i < plc.length; i += 1) {
        const p = plc[i];
        const tid = p.MannschaftsID;
        const tn = this.getTeamName(tid);
        let sid = '';
        if (p.SID) {
          sid = ` (${p.SID})`;
        }

        ml.push({ ID: p.ID, Name: `${tn}: ${p.Vorname} ${p.Nachname}${sid}` });
      }
    }

    const P = this.getVarWert('S36');

    return (
      <div className="input-group">
        <input type="text" className="form-control" aria-label="..." value={P} onChange={this.handlePlayerSelection.bind(this)} />
        <DlgTable
          label1="Spieler wahl ..."
          data-toggle="tooltip"
          data-placement="right"
          toolTip="Spieler wählen"
          reacticon={<IoMdArrowDropdown size="1.5em" />}
          modalID="Modal-M5"
          data={ml}
          wahl={this.handlePlayerSelection.bind(this)}
        />
      </div>
    );
  }
}

Spieler.propTypes = {
  textvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  teamList: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam1: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam2: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default Spieler;
