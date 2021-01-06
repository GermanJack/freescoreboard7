import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Strafe from '../../StdComponents/Strafe';

class Strafen extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  getVarWert(v) {
    const i = this.props.textvariablen.map((t) => t.ID).indexOf(v);
    return i !== -1 ? this.props.textvariablen[i].Wert : '0';
  }

  reset() {
    // console.log('Strafen_Reset');
    this.props.websocketSend({ Domain: 'KO', Type: 'bef', Command: 'Strafen_Reset' });
  }

  render() {
    const m1 = this.getVarWert('S01');
    const m2 = this.getVarWert('S02');
    return (
      <div>
        <Strafe
          team="1"
          teamName={m1}
          playerlist={this.props.playerlistTeam1}
          penalties={this.props.penalties}
          variable="T04"
          tabellenvariablen={this.props.tabellenvariablen}
          anzeigetabellen={this.props.anzeigetabellen}
          websocketSend={this.props.websocketSend.bind(this)}
        />
        <Strafe
          team="2"
          teamName={m2}
          playerlist={this.props.playerlistTeam2}
          penalties={this.props.penalties}
          variable="T05"
          tabellenvariablen={this.props.tabellenvariablen}
          anzeigetabellen={this.props.anzeigetabellen}
          websocketSend={this.props.websocketSend.bind(this)}
        />
      </div>
    );
  }
}

Strafen.propTypes = {
  textvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  tabellenvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  penalties: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam1: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam2: PropTypes.arrayOf(PropTypes.object).isRequired,
  anzeigetabellen: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default Strafen;
