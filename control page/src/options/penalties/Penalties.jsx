import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GoPlus, GoDash, GoSync } from 'react-icons/go';
import Table from '../../StdComponents/Table';
import Dropdown from '../../StdComponents/Dropdown';
import DlgSndSelection from '../../StdComponents/DlgSndSelection';
import DlgName from '../../StdComponents/DlgName';

class Penalties extends Component {
  constructor(props) {
    super(props);
    this.state = {
      penTypes: [],
      selpenaltie: this.props.penalties[0],
    };
  }

  componentDidMount() {
    this.setState({
      penTypes: [
        { No: '01', Name: 'Verwarnung' },
        { No: '02', Name: 'Verweis' },
        { No: '03', Name: 'Zeitstrafe' },
      ],
    });
  }

  componentDidUpdate(prevProps) {
    if (prevProps.penalties !== this.props.penalties) {
      const a = this.props.penalties.length;
      const b = this.props.penalties.map((x) => x.ID).includes(this.state.selpenaltie.ID);
      if (a > 0 && !b) {
        // eslint-disable-next-line react/no-did-update-set-state
        this.setState({ selpenaltie: this.props.penalties[0] });
      }
    }
  }

  getTypeName(e) {
    const t = this.state.penTypes.find((x) => x.No === e);
    return (t ? t.Name : null);
  }

  setPropValue(property, e) {
    const et = this.state.selpenaltie;
    switch (property) {
      case 'Art':
        et.Art = this.state.penTypes.find((x) => x.Name === e).No;
        break;
      case 'Sekunden':
        et.Sekunden = e.target.value;
        break;
      case 'EndeTon':
        et.EndeTon = e;
        break;
      case 'TonCountdown':
        et.TonCountdown = e;
        break;
      case 'Countdowndauer':
        et.Countdowndauer = e.target.value;
        break;

      default:
    }

    this.props.websocketSend({ Type: 'bef', Command: 'SetPenaltie', Value1: JSON.stringify(et) });
  }

  setOptValue(prop, e) {
    this.props.websocketSend({
      Type: 'bef',
      Command: 'SetOptValue',
      Value1: prop,
      Value2: e.target.value,
    });
  }

  rowClick(id) {
    // console.log(e);
    const ml = this.props.penalties;
    const mi = ml.findIndex((x) => x.ID === id);
    const m = this.props.penalties[mi];
    this.setState({ selpenaltie: m });
  }

  delPenaltie() {
    this.props.websocketSend({ Type: 'bef', Command: 'DelPenaltie', Value1: this.state.selpenaltie.Bezeichnung });
  }

  addPenaltie(e) {
    this.props.websocketSend({ Type: 'bef', Command: 'AddPenaltie', Value1: e });
  }

  render() {
    const pID = this.state.selpenaltie ? this.state.selpenaltie.ID : '';
    let snd = 'd-none';
    if (this.state.selpenaltie) {
      if (this.state.selpenaltie.Art === '03') {
        snd = 'd-flex flex-row align-items-baseline mb-1';
      }
    }

    let parallel = 0;
    if (this.props.options.length > 0) {
      const s = this.props.options.find((x) => x.Prop === 'Parallelstrafen');
      parallel = s ? s.Value : 0;
    }

    return (

      <div className="p-1 ml-1">
        <u>Strafen</u>

        <div className="d-flex flex-row m-1">
          <div className="">
            Anzahl paralleler Strafen:
          </div>
          <input
            title="Parallele Strafen"
            type="number"
            className="ml-2 mr-2"
            style={{ width: '50px' }}
            min="0"
            max="100"
            value={parallel}
            onChange={this.setOptValue.bind(this, 'Parallelstrafen')}
          />
          <div className="">
            (0 = keine Begrenzung)
          </div>
        </div>

        <div className="d-flex flex-row">
          <Table
            daten={this.props.penalties}
            cols={[{ Column: 'Bezeichnung', Label: 'Bezeichnung' }]}
            chk="Selected"
            chkid={[pID]}
            radiogrp="penalties"
            rowClick={this.rowClick.bind(this)}
          />

          <div className="d-flex flex-column ml-3">
            <div className="d-flex flex-row align-items-baseline mb-1">
              <DlgName
                label1="Name für neue Strafe:"
                text=""
                values={this.props.penalties.map((x) => x.Bezeichnung)}
                icon={<GoPlus size="1.2em" />}
                class="btn btn-outline-secondary btn-icon p-0 pl-1 pr-1 pb-1 ml-2 mr-2"
                toolTip="neue Strafe"
                modalID="Modal-P1"
                allowSpace
                name={this.addPenaltie.bind(this)}
              />

              <button
                type="button"
                className="btn btn-outline-secondary btn-icon p-0 pl-1 pr-1 pb-1 ml-2 mr-2"
                title="Strafe löschen"
                onClick={this.delPenaltie.bind(this)}
              >
                <GoDash size="1.2em" />
              </button>
            </div>

            <div className="d-flex flex-row align-items-baseline mb-1">
              <div>
                <u>{this.state.selpenaltie ? `${this.state.selpenaltie.Bezeichnung} :` : ''}</u>
              </div>
            </div>

            <div className="d-flex flex-row align-items-baseline mb-1 mr-1">
              <div className="mr-1">
                Art :
              </div>
              <Dropdown
                className=""
                values={this.state.penTypes.map((p) => p.Name)}
                value={this.getTypeName(this.state.selpenaltie ? this.state.selpenaltie.Art : null)}
                wahl={this.setPropValue.bind(this, 'Art')}
              />
            </div>

            <div className={snd}>
              <div>
                Dauer :
              </div>
              <input
                title="Dauer"
                type="number"
                className="ml-2 mr-2"
                style={{ width: '100px' }}
                min="0"
                max="100000"
                value={this.state.selpenaltie ? this.state.selpenaltie.Sekunden : null}
                onChange={this.setPropValue.bind(this, 'Sekunden')}
              />
              <div>
                Sekunden
              </div>
            </div>

            <div className={snd}>
              <div>
                Countdown-Ton in den letzten
              </div>
              <input
                title="CountDownDauer"
                type="number"
                className="ml-2 mr-2"
                style={{ width: '100px' }}
                min="0"
                max="60"
                value={this.state.selpenaltie ? this.state.selpenaltie.Countdowndauer : 0}
                onChange={this.setPropValue.bind(this, 'Countdowndauer')}
              />
              <div>
                Sekunden.
              </div>
            </div>

            <div className={snd}>
              <div>
                Countdown-Ton :
              </div>
              <div className="ml-2 pl-1 pr-1 border">
                {this.state.selpenaltie ? this.state.selpenaltie.TonCountdown : null}
              </div>
              <DlgSndSelection
                label1="Cownt Dorn Ton wählen:"
                text=""
                className="ml-2 mr-2"
                data-toggle="tooltip"
                data-placement="right"
                toolTip="Audiodatei wählen"
                modalID="Modal-S5"
                values={this.props.soundList}
                selection={this.setPropValue.bind(this, 'TonCountdown')}
              />
            </div>

            <div className={snd}>
              <div>
                Schlußton :
              </div>
              <div className="ml-2 pl-1 pr-1 border">
                {this.state.selpenaltie ? this.state.selpenaltie.EndeTon : null}
              </div>
              <DlgSndSelection
                label1="Ende Ton wählen:"
                text=""
                className="ml-2 mr-2"
                data-toggle="tooltip"
                data-placement="right"
                toolTip="Audiodatei wählen"
                modalID="Modal-S4"
                values={this.props.soundList}
                selection={this.setPropValue.bind(this, 'EndeTon')}
              />
            </div>

          </div>
        </div>
      </div>
    );
  }
}

Penalties.propTypes = {
  penalties: PropTypes.oneOfType(PropTypes.object),
  soundList: PropTypes.oneOfType(PropTypes.object),
  options: PropTypes.oneOfType(PropTypes.object),
  websocketSend: PropTypes.func,
};

Penalties.defaultProps = {
  penalties: [],
  soundList: [],
  options: [],
  websocketSend: () => {},
};

export default Penalties;
