import React, { Component } from 'react';
import PropTypes from 'prop-types';
import DropdownTimer from '../../StdComponents/DropdownTimer';
import TimerEvents from './TimerEvents';
import Table from '../../StdComponents/Table';
import MinuteTime from '../../StdComponents/MinuteTime';

class Timer extends Component {
  constructor(props) {
    super(props);
    this.state = {
      timerstate: [{ Nr: 0, Name: 'nicht läuft' }, { Nr: 1, Name: 'läuft' }],
      seltimer: null,
      seltimerevent: '',
    };
  }

  componentDidMount() {
    if (!this.state.seltimer) {
      this.onTimerSelect(this.props.timer[0].ID);
    } else {
      const sid = this.state.seltimer.ID;
      const da = this.props.timer.map((x) => x.ID).includes(sid);
      if (!da) {
        this.onTimerSelect(this.props.timer[0].ID);
      }
    }
  }

  componentDidUpdate() {
    if (this.props.timerevents) {
      if (this.props.timerevents.length > 0) {
        if (!this.state.seltimerevent) {
          this.timerEventSel(this.props.timerevents[0].ID);
        } else {
          const sid = this.state.seltimerevent.ID;
          const da = this.props.timerevents.map((x) => x.ID).includes(sid);
          if (!da) {
            this.timerEventSel(this.props.timerevents[0].ID);
          }
        }
      }
    }
  }

  onTimerSelect(id) {
    const ml = this.props.timer;
    const mi = ml.findIndex((x) => x.ID === id);
    const m = this.props.timer[mi];
    this.setState({ seltimer: m });
    this.setState({ seltimerevent: '' });
    // this.setState({ timerevents: [] });
    this.props.websocketSend({
      Type: 'req',
      Command: 'Timerevents',
      Value1: m.Nr,
    });
  }

  getDepTimerName(e) {
    let ret = '[inaktiv]';
    if (e !== 0) {
      const t = this.props.timer.find((x) => x.Nr === e);
      ret = t ? t.Name : null;
    }
    return ret;
  }

  getDepTimerNr(e) {
    if (e === '[inaktiv]') {
      return 0;
    }
    const t = this.state.seltimer.find((x) => x.Name === e);
    return t ? t.Nr : null;
  }

  getDepTimerStatusNr(e) {
    let ret = true;
    if (e !== 'läuft') {
      ret = false;
    }

    return ret;
  }

  getDepTimerStatusTxt(e) {
    let ret = 'läuft';
    if (!e) {
      ret = 'nicht läuft';
    }

    return ret;
  }

  getBoolFromInt(e) {
    let ret = true;
    if (e === 0) {
      ret = false;
    }

    return ret;
  }

  getIntFromBool(e) {
    let ret = 0;
    if (e) {
      ret = 1;
    }

    return ret;
  }

  getTimerNames(e) {
    const arr = [{ Nr: 0, Name: '[inaktiv]' }];
    const arr2 = this.props.timer.filter((z) => z.Name !== e);
    return arr.concat(arr2);
  }

  setPropValue(property, e) {
    const et = this.state.seltimer;
    switch (property) {
      case 'Kontrolanzeige':
        et.Kontrolanzeige = e.target.checked;
        break;
      case 'StartSekunden':
        et.StartSekunden = e;
        break;
      case 'Countdown':
        et.Countdown = e.target.checked;
        break;
      case 'MinutenDarstellung':
        et.MinutenDarstellung = e.target.checked;
        break;
      case 'DisplayDynamisch':
        et.DisplayDynamisch = e.target.checked;
        break;
      case 'AbhaengigerTimerNr':
        et.AbhängigeTimerNr = e;
        break;
      case 'AbhaengigerTimerStatus':
        et.AbhängigeTimerStatus = false;
        if (e === 1) {
          et.AbhängigeTimerStatus = true;
        }
        break;
      default:
    }

    this.props.websocketSend({ Type: 'bef', Command: 'SetTimer', Value1: JSON.stringify(et) });
  }

  selectfirsttimerevent() {
    if (this.props.timerevents.length > 0) {
      const c = this.props.timerevents[0].ID;
      this.timerEventSel(c);
    }
  }

  refreschTimerEvents(pl) {
    const pid = this.props.timerevent ? this.props.timerevent.ID : null;
    const plids = pl.map((a) => a.ID);
    if (pid) {
      // check if selected event is still in list
      // when a event is deleted this is not the case any more
      // if selected event is not found then delete selection
      if (!plids.includes(pid)) {
        this.setState({ seltimerevent: null }, () => {
          this.selectfirsttimerevent(pl);
        });
      } else {
        this.timerEventSel(pid.ID);
      }
    } else {
      this.selectfirsttimerevent();
    }
  }

  timerEventSel(id) {
    // let intNr = parseInt(id, 10);
    const ml = this.props.timerevents;
    const mi = ml.findIndex((x) => x.ID === id);
    const m = this.props.timerevents[mi];
    this.setState({ seltimerevent: m });
  }

  secChange(p, e) {
    let sek = e.target.value;
    if (sek === '') {
      sek = 0;
    }
    this.setPropValue(p, sek);
  }

  render() {
    // Zeiteingabe
    let ze = (
      <div className="d-flex flex-row">
        <input
          title="Startzeit"
          type="number"
          className="form-control mt-1 mb-1 ml-2 mr-2"
          style={{ width: '100px' }}
          min="0"
          max="100000"
          value={this.state.seltimer ? this.state.seltimer.StartSekunden : ''}
          onChange={this.secChange.bind(this, 'StartSekunden')}
        />
        <div>Sekunden</div>
      </div>
    );

    if (this.state.seltimer ? this.state.seltimer.MinutenDarstellung : null) {
      ze = (
        <MinuteTime
          seconds={this.state.seltimer ? this.state.seltimer.StartSekunden : 0}
          TimeSet={this.setPropValue.bind(this, 'StartSekunden')}
        />
      );
    }

    // console.log(this.props.zeiten)
    const til = this.props.timer ? this.props.timer.sort((a, b) => (a.Nr - b.Nr)) : null;

    const tev = this.state.seltimer
      ? (
        <TimerEvents
          timer={this.props.timer}
          seltimer={this.state.seltimer}
          timerevents={this.props.timerevents}
          seltimerevent={this.state.seltimerevent}
          timerEventSel={this.timerEventSel.bind(this)}
          soundList={this.props.soundList}
          pages={this.props.pages}
          websocketSend={this.props.websocketSend}
        />
      )
      : null;

    return (
      <div className="p-1 ml-1">
        <u>Uhren</u>
        <div className="d-flex flex-row flex-grow-1">
          <Table
            daten={til}
            cols={[{ Column: 'Name', Label: 'Name' }]}
            chk="Selected"
            chkid={this.state.seltimer ? [this.state.seltimer.ID] : []}
            radiogrp="timer"
            rowClick={this.onTimerSelect.bind(this)}
          />

          <div className="d-flex flex-column flex-grow-1 p-2">
            <div className="">
              <div className={this.state.seltimer ? 'visible' : 'invisible'}>

                <div>
                  <u>
                    {this.state.seltimer ? this.state.seltimer.Name : null}
                  </u>
                </div>
                <div className="d-flex flex-row align-items-baseline">
                  <div className="mr-1r">
                    Uhr aktiv :
                  </div>
                  <label htmlFor="x" className="switch pl-2 ml-1 text-center">
                    <input
                      type="checkbox"
                      id="x"
                      checked={this.state.seltimer ? this.state.seltimer.Kontrolanzeige : null}
                      onChange={this.setPropValue.bind(this, 'Kontrolanzeige')}
                    />
                    <span className="slider round" />
                  </label>
                </div>

                <div className="d-flex flex-row align-items-baseline">
                  <div className="mr-1">
                    Minutendarstellung :
                  </div>
                  <label htmlFor="y" className="switch pl-2 ml-1 text-center">
                    <input
                      type="checkbox"
                      id="y"
                      checked={this.state.seltimer ? this.state.seltimer.MinutenDarstellung : null}
                      onChange={this.setPropValue.bind(this, 'MinutenDarstellung')}
                    />
                    <span className="slider round" />
                  </label>
                </div>

                <div className="d-flex flex-row align-items-baseline">
                  <div className="mb-1 mr-1">
                    Startzeit :
                  </div>
                  {ze}
                </div>

                <div className="d-flex flex-row align-items-baseline">
                  <div className="mr-1">
                    Countdown :
                  </div>
                  <label htmlFor="z" className="switch pl-2 ml-1 text-center">
                    <input
                      type="checkbox"
                      id="z"
                      checked={this.state.seltimer ? this.state.seltimer.Countdown : null}
                      onChange={this.setPropValue.bind(this, 'Countdown')}
                    />
                    <span className="slider round" />
                  </label>
                </div>

                <div className="d-flex flex-row align-items-baseline mb-1">
                  <div className="mr-1">
                    Nur Anzeigen wenn Uhr läuft :
                  </div>
                  <label htmlFor="g" className="switch pl-2 ml-1 vtext-center">
                    <input
                      type="checkbox"
                      id="g"
                      checked={this.state.seltimer ? this.state.seltimer.DisplayDynamisch : null}
                      onChange={this.setPropValue.bind(this, 'DisplayDynamisch')}
                    />
                    <span className="slider round" />
                  </label>
                </div>

              </div>

              <div className="d-flex flex-row align-items-baseline">
                <div className="pr-2">
                  {`${this.state.seltimer ? this.state.seltimer.Name : null} läuft nur wenn :`}
                </div>
                <DropdownTimer
                  className=""
                  values={this.getTimerNames(this.state.seltimer
                    ? this.state.seltimer.Name : null)}
                  value={this.getDepTimerName(this.state.seltimer
                    ? this.state.seltimer.AbhängigeTimerNr : null)}
                  selection={this.setPropValue.bind(this, 'AbhaengigerTimerNr')}
                />
                <DropdownTimer
                  className=""
                  values={this.state.timerstate}
                  value={this.getDepTimerStatusTxt(this.state.seltimer
                    ? this.state.seltimer.AbhängigeTimerStatus : null)}
                  selection={this.setPropValue.bind(this, 'AbhaengigerTimerStatus')}
                />
              </div>

            </div>

            <div className="d-flex flex-row flex-grow-1">
              <div className="d-flex flex-column">
                {tev}
              </div>
            </div>

          </div>
        </div>
      </div>
    );
  }
}

Timer.propTypes = {
  timer: PropTypes.oneOfType(PropTypes.object),
  timerevents: PropTypes.arrayOf(PropTypes.object),
  timerevent: PropTypes.oneOfType(PropTypes.object),
  soundList: PropTypes.oneOfType(PropTypes.object),
  pages: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func,
};

Timer.defaultProps = {
  timer: null,
  timerevents: [],
  timerevent: [],
  soundList: [],
  websocketSend: () => {},
};

export default Timer;
