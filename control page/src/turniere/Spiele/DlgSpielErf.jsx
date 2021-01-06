import React, { Component } from 'react';
import PropTypes from 'prop-types';

import { GiWhistle } from 'react-icons/gi';
import NumUpDownsmall from '../../StdComponents/NumUpDown_small';
import CtrlEndMatch from '../../StdComponents/CtrlEndMatch';

class DlgSpielErf extends Component {
  constructor() {
    super();
    this.state = {
      ToreA: 0,
      ToreB: 0,
      SpielEndeOption: '1',
      manualChange: false,
    };
  }

  componentDidUpdate() {
    if (this.props.Spiel.ToreA !== this.state.ToreA && this.state.manualChange === false) {
      // eslint-disable-next-line react/no-did-update-set-state
      this.setState({ ToreA: this.props.Spiel.ToreA });
    }

    if (this.props.Spiel.ToreB !== this.state.ToreB && this.state.manualChange === false) {
      // eslint-disable-next-line react/no-did-update-set-state
      this.setState({ ToreB: this.props.Spiel.ToreB });
    }
  }

  setWert(e) {
    let z = 2;
    // eslint-disable-next-line react/no-access-state-in-setstate
    if (e.variable === 'ToreA') {
      this.setState({ manualChange: true });
      let wert = this.state.ToreA;
      if (e.funcID === '+') {
        wert += 1;
      } else {
        wert -= 1;
      }
      this.setState({ ToreA: wert });
    }

    if (e.variable === 'ToreB') {
      this.setState({ manualChange: true });
      let wert = this.state.ToreB;
      if (e.funcID === '+') {
        wert += 1;
      } else {
        wert -= 1;
      }
      this.setState({ ToreB: wert });
    }
  }

  CtrlEndMatchChange(e) {
    this.setState({ SpielEndeOption: e.target.value });
  }

  handleClickOK() {
    this.props.websocketSend(
      {
        Type: 'bef',
        Command: 'SetMatch',
        Property: this.props.Spiel.ID,
        Value1: this.state.ToreA,
        Value2: this.state.ToreB,
        Value3: this.state.SpielEndeOption,
      },
    );
    this.ini();
  }

  ini() {
    this.setState({ manualChange: false });
    this.setState({ ToreA: 0 });
    this.setState({ ToreB: 0 });
    this.setState({ SpielEndeOption: '1' });
  }

  render() {
    const cn = this.props.btnclassname ? this.props.btnclassname : 'btn btn-primary btn-icon border-dark p-0 mr-1';
    let icon = '';
    if (this.props.icon !== '') {
      icon = (
        <GiWhistle size="1.2em" />
      );
    }

    const ico = this.props.icon ? icon : this.props.reacticon;

    let disableSave = false;
    let meldung = '';

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
                <div className="row mb-2">
                  <div className="d-flex flex-column">
                    <div className="form form-control mb-1">
                      {this.props.Spiel.IstMannA}
                    </div>
                    <NumUpDownsmall
                      variable="ToreA"
                      count={this.state.ToreA}
                      click={this.setWert.bind(this)}
                    />
                  </div>
                  <div className="d-flex flex-column ml-3">
                    <div className="form form-control mb-1">
                      {this.props.Spiel.IstMannB}
                    </div>
                    <NumUpDownsmall
                      variable="ToreB"
                      count={this.state.ToreB}
                      click={this.setWert.bind(this)}
                    />
                  </div>
                </div>
                <CtrlEndMatch
                  SpielEndeOption={this.state.SpielEndeOption}
                  CtrlEndMatchChange={this.CtrlEndMatchChange.bind(this)}
                />
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

DlgSpielErf.propTypes = {
  modalID: PropTypes.string,
  toolTip: PropTypes.string,
  reacticon: PropTypes.string,
  btnclassname: PropTypes.string,
  icon: PropTypes.string,
  label: PropTypes.string,

  Spiel: PropTypes.objectOf.TSpiele.isRequired,
  websocketSend: PropTypes.func.isRequired,
};

DlgSpielErf.defaultProps = {
  modalID: 'filedlg',
  toolTip: '',
  reacticon: '',
  btnclassname: '',
  icon: '',
  label: 'Ereignis',
};

export default DlgSpielErf;
