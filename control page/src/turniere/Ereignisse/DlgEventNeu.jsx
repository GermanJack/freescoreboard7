import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { AiOutlineFileAdd } from 'react-icons/ai';
import DlgEvent from './DlgEvent';

import TEreignisse from '../dataClasses/TEreignisse';
import ClsTurnier from '../dataClasses/ClsTurnier';

class DlgEventNeu extends Component {
  constructor() {
    super();
    this.state = {
      Ev: new TEreignisse(),
    };
  }

  componentDidMount() {
    this.ini();
  }

  setWert(merkmal, wert) {
    // eslint-disable-next-line react/no-access-state-in-setstate
    const x = this.state.Ev;
    if (merkmal === 'Ereignisart') {
      x.Ereignistyp = wert;
    }

    if (merkmal === 'Spiel') {
      x.Spiel = wert;
    }

    if (merkmal === 'Mannschaft') {
      x.Mannschaft = wert;
    }

    if (merkmal === 'Spielzeit') {
      x.Spielzeit = wert;
    }

    if (merkmal === 'Abschnitt') {
      x.Spielabschnitt = wert.target.value;
    }

    if (merkmal === 'Spieler') {
      x.Spieler = wert;
    }

    if (merkmal === 'Details') {
      x.Details = wert.target.value;
    }

    this.setState({ Ev: x });
  }

  handleClickOK() {
    const sysjson = JSON.stringify(this.state.Ev);
    this.props.websocketSend({ Type: 'bef', Command: 'AddEvent', Value1: sysjson });
    this.ini();
  }

  ini() {
    if (this.props.textvariablen.length > 0) {
      const e = new TEreignisse(
        0,
        this.props.textvariablen.find((x) => x.ID === 'S13').Wert,
        0,
        '',
        '00:00',
        true,
        0,
        '',
        '',
        '',
        1,
        this.props.textvariablen.find((x) => x.ID === 'S13').Wert,
      );
      this.setState({ Ev: e });
    }
  }

  render() {
    const cn = this.props.btnclassname ? this.props.btnclassname : 'btn btn-primary btn-icon border-dark p-0 mr-1';
    let icon = '';
    if (this.props.icon !== '') {
      icon = (
        <AiOutlineFileAdd size="1.2em" />
      );
    }

    const ico = this.props.icon ? icon : this.props.reacticon;

    let disableSave = false;
    let meldung = '';
    if (this.state.Ev.Ereignistyp === '' || this.state.Ev.Ereignistyp === undefined) {
      meldung = 'Ereignisart fehlt';
      disableSave = true;
    }

    if (this.state.Ev.Spiel === '' || this.state.Ev.Spiel === undefined) {
      meldung = 'SpielNr. fehlt';
      disableSave = true;
    }

    const et = this.state.Ev.Ereignistyp;
    if (et === '05' || et === '06' || et === '07') {
      if (this.state.Ev.Mannschaft === '' || this.state.Ev.Mannschaft === undefined) {
        meldung = 'Mannschaft fehlt';
        disableSave = true;
      }
    }

    return (
      <div className="d-flex flex-column">
        <button
          type="button"
          className={cn}
          data-placement="right"
          title={this.props.toolTip}
          data-toggle="modal"
          data-target={`#${this.props.modalID}`}
        >
          {ico}
        </button>

        <div className="modal fade" id={this.props.modalID} tabIndex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
          <div className="modal-dialog">
            <div className="modal-content">

              <div className="modal-header">
                <h5 className="modal-title text-dark">{this.props.label}</h5>
                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">
                    &times;
                  </span>
                </button>
              </div>

              <div className="modal-body">
                <div className="d-flex flex-column">
                  <DlgEvent
                    Event={this.state.Ev}
                    spiele={this.props.spiele}
                    events={this.props.events}
                    textvariablen={this.props.textvariablen}
                    teamList={this.props.teamList}
                    playerListAll={this.props.playerListAll}
                    setWert={this.setWert.bind(this)}
                    turnier={this.props.turnier}
                  />
                </div>
              </div>

              <div className="modal-footer">
                <div>{meldung}</div>
                <button
                  type="button"
                  className="btn btn-primary save"
                  disabled={disableSave}
                  onClick={this.handleClickOK.bind(this)}
                  data-dismiss="modal"
                >
                  OK
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

DlgEventNeu.propTypes = {
  modalID: PropTypes.string,
  toolTip: PropTypes.string,
  reacticon: PropTypes.string,
  btnclassname: PropTypes.string,
  icon: PropTypes.string,
  label: PropTypes.string,
  teamList: PropTypes.arrayOf(PropTypes.object),
  textvariablen: PropTypes.arrayOf(PropTypes.object),
  websocketSend: PropTypes.func.isRequired,
  playerListAll: PropTypes.arrayOf(PropTypes.object).isRequired,
  spiele: PropTypes.arrayOf(PropTypes.object).isRequired,
  events: PropTypes.arrayOf(PropTypes.object).isRequired,
  turnier: PropTypes.instanceOf(ClsTurnier).isRequired,
};

DlgEventNeu.defaultProps = {
  modalID: 'filedlg',
  toolTip: '',
  reacticon: '',
  btnclassname: '',
  icon: '',
  label: 'Ereignis',
  teamList: [],
  textvariablen: [],
};

export default DlgEventNeu;
