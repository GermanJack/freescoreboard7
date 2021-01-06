import React, { Component } from 'react';
import PropTypes from 'prop-types';
import DlgWarning from '../../StdComponents/DlgWarning';
import DlgTurnierNeu from './DlgTurnierNeu';

class Turniermenue extends Component {
  constructor() {
    super();
    this.state = {
    };

    this.treeRef = React.createRef();
  }

  clickDel() {
    this.props.turnierDelete('System');
  }

  clickStart() {
    this.props.websocketSend({ Type: 'bef', Command: 'SetTurnierID', Value1: this.props.turnierID });
  }

  clickEnd() {
    this.props.websocketSend({ Type: 'bef', Command: 'SetTurnierID', Value1: 0 });
  }

  clickBericht() {
    console.log('Bericht');
  }

  clickGrafik() {
    this.props.grafik();
  }

  render() {
    return (
      <div className="d-flex flex-column">

        <div className="d-flex flex-row p-1">
          <div className="mr-1">
            <DlgTurnierNeu
              label1="neues Turnier"
              toolTip="neues Turnier"
              modalID="Modal-Turn1"
              namensliste={this.props.namensliste}
              websocketSend={this.props.websocketSend.bind(this)}
              systeme={this.props.systeme}
              teamList={this.props.teamList}
            />
          </div>

          {/* objekt löschen */}
          <DlgWarning
            label1="Turnier unwiederruflich löschen?"
            toolTip="Turnier löschen"
            modalID="Modal-Turn2"
            name={this.clickDel.bind(this)}
          />
          <button type="button" className="btn btn-primary border-dark m-1" onClick={this.clickStart.bind(this)}>
            Starten
          </button>
          <button type="button" className="btn btn-primary border-dark m-1" onClick={this.clickEnd.bind(this)}>
            Beenden
          </button>
          <button type="button" className="btn btn-primary border-dark m-1" onClick={this.clickBericht.bind(this)}>
            Bericht
          </button>
          <button type="button" className="btn btn-primary border-dark m-1" onClick={this.clickGrafik.bind(this)}>
            Grafik
          </button>
        </div>
      </div>
    );
  }
}

Turniermenue.propTypes = {
  namensliste: PropTypes.arrayOf(PropTypes.string),
  systeme: PropTypes.arrayOf(PropTypes.object),
  teamList: PropTypes.arrayOf(PropTypes.object),
  turnierID: PropTypes.number,
  websocketSend: PropTypes.func,
  turnierDelete: PropTypes.func,
  grafik: PropTypes.func,
};

Turniermenue.defaultProps = {
  namensliste: [],
  systeme: [],
  teamList: [],
  turnierID: 0,
  websocketSend: () => {},
  turnierDelete: () => {},
  grafik: () => {},
};

export default Turniermenue;
