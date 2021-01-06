import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GoPlus, GoDash } from 'react-icons/go';
import Table from '../../StdComponents/Table';
import DropupTimer from '../../StdComponents/DropupTimer';
import DlgSndSelection from '../../StdComponents/DlgSndSelection';
import MinuteTime from '../../StdComponents/MinuteTime';
import DropdownWithID from '../../StdComponents/DropdownWithID';

class TimerEvents extends Component {
  constructor() {
    super();
    this.state = {
      evtType: [
        { Nr: 0, Name: 'Uhr stoppen' },
        { Nr: 1, Name: 'andere Uhr starten' },
        { Nr: 2, Name: 'andere Uhr stoppen' },
        { Nr: 3, Name: 'Audiodatei abspielen' },
        { Nr: 4, Name: 'Anzeige wechseln' }],
    };
  }

  getTimerNames(e) {
    let arr = [];
    arr = this.props.timer.filter((z) => z.Name !== e);
    return arr;
  }

  getDepTimerName(e) {
    let ret = '[inaktiv]';
    if (e !== 0) {
      const t = this.props.timer.find((x) => x.Nr === e);
      if (t) {
        ret = t.Name;
      } else {
        ret = null;
      }
    }

    return ret;
  }

  getDepEventName(e) {
    const t = this.state.evtType.find((x) => x.Nr === e);
    return t ? t.Name : null;
  }

  setPropValue(property, e) {
    const et = this.props.seltimerevent;
    switch (property) {
      case 'Active':
        et.Active = e.target.checked;
        break;
      case 'Sekunden':
        et.Sekunden = e;
        break;
      case 'Eventtype':
        et.Eventtype = e;
        break;
      case 'Soundfile':
        et.Soundfile = e;
        break;
      case 'AndereTimerNr':
        et.AndereTimerNr = e;
        break;
      case 'Page':
        et.Layer = e;
        break;

      default:
    }

    this.props.websocketSend({ Type: 'bef', Command: 'SetTimerEvent', Value1: JSON.stringify(et) });
  }

  rowClick(e) {
    this.props.timerEventSel(e);
  }

  delTimerEvent() {
    this.props.websocketSend({
      Type: 'bef',
      Command: 'DelTimerEvent',
      Value1: this.props.seltimerevent.ID,
      Value2: this.props.seltimer.Nr,
    });
  }

  addTimerEvent() {
    this.props.websocketSend({ Type: 'bef', Command: 'AddTimerEvent', Value1: this.props.seltimer.Nr });
  }

  secChange(p, e) {
    this.setPropValue(p, e.target.value);
  }

  Zeit(e, tnr) {
    const t = this.props.timer.find((x) => x.Nr === tnr);
    const te = this.props.timerevents.find((x) => x.ID === e);

    let ret = '';
    if (t.MinutenDarstellung) {
      const m = Math.trunc(te.Sekunden / 60);
      const s = te.Sekunden - (m * 60);
      ret = `${m} : ${s}`;
    } else {
      ret = te.Sekunden;
    }

    return ret;
  }

  render() {
    const sID = this.props.seltimerevent ? this.props.seltimerevent.ID : 0;
    const snd = this.props.seltimerevent.Eventtype === 3 ? 'd-flex flex-row align-items-baseline mb-1' : 'd-none';
    const hlp = this.props.seltimerevent.Eventtype;
    const uhr = hlp === 1 || hlp === 2 ? 'd-flex flex-row align-items-baseline mb-1' : 'd-none';
    const pagesel = this.props.seltimerevent.Eventtype === 4 ? 'd-flex flex-row align-items-baseline mb-1' : 'd-none';

    let ze = (
      <div className="d-flex flex-row">
        <input
          title="Ereigniszeit"
          type="number"
          className="form-control mt-1 ml-2 mr-2"
          style={{ width: '100px' }}
          min="0"
          max="100000"
          value={this.props.seltimerevent ? this.props.seltimerevent.Sekunden : null}
          onChange={this.secChange.bind(this, 'Sekunden')}
        />
        <div>
          Sekunden
        </div>
      </div>
    );

    if (this.props.seltimer.MinutenDarstellung) {
      ze = (
        <MinuteTime
          seconds={this.props.seltimerevent ? this.props.seltimerevent.Sekunden : null}
          TimeSet={this.setPropValue.bind(this, 'Sekunden')}
        />
      );
    }

    const list = this.props.timerevents.map((x) => (
      {
        ID: x.ID,
        Sekunden: [this.Zeit(x.ID, x.TimerNr)],
        Eventtype: this.state.evtType.find((y) => y.Nr === x.Eventtype).Name,
      }
    ));

    let Pages = [];
    if (this.props.pages.length > 0) {
      Pages = this.props.pages.map((x) => (
        {
          ID: x.ID,
          Text: x.PageName,
        }
      ));
    }

    return (

      <div className="mt-1">
        <u>
          {`Zeitereignisse ${this.props.seltimer.Name}`}
        </u>
        <div className="d-flex flex-row">
          <Table
            daten={list}
            cols={[{ Column: 'Sekunden', Label: 'Zeit' }, { Column: 'Eventtype', Label: 'Ereignistyp' }]}
            chk="Selected"
            chkid={[sID]}
            radiogrp="timerevents"
            rowClick={this.rowClick.bind(this)}
            evtType={this.state.evtType}
          />

          <div className="d-flex flex-column ml-3">
            <div className="d-flex flex-row align-items-baseline mb-1">
              <button
                type="button"
                className="btn btn-outline-secondary btn-icon p-0 pl-1 pr-1 pb-1 mr-2"
                onClick={this.addTimerEvent.bind(this)}
              >
                <GoPlus size="1.2em" />
              </button>
              <button
                type="button"
                className="btn btn-outline-secondary btn-icon p-0 pl-1 pr-1 pb-1 mr-2"
                onClick={this.delTimerEvent.bind(this)}
              >
                <GoDash size="1.2em" />
              </button>
            </div>

            <div className="d-flex flex-row align-items-baseline mb-1">
              <div>
                Ereignis aktiv :
              </div>

              <label htmlFor="x" className="switch pl-2 ml-1 text-center">
                <input
                  type="checkbox"
                  id="x"
                  checked={this.props.seltimerevent ? this.props.seltimerevent.Active : null}
                  onChange={this.setPropValue.bind(this, 'Active')}
                />
                <span className="slider round" />
              </label>
            </div>

            <div className="d-flex flex-row align-items-baseline mb-1">
              <div className="mr-1">
                Ereigniszeit :
              </div>
              {ze}
            </div>

            <div className="d-flex flex-row align-items-baseline mb-1">
              <div>
                Ereignisart :
              </div>

              <DropupTimer
                className="form-control form-control-sm ml-2 mr-2"
                values={this.state.evtType}
                value={this.getDepEventName(this.props.seltimerevent
                  ? this.props.seltimerevent.Eventtype : null)}
                selection={this.setPropValue.bind(this, 'Eventtype')}
              />
            </div>

            <div className={snd}>
              <div>
                Audiodatei :
              </div>

              <div className="ml-2 pl-1 pr-1 border">
                {this.props.seltimerevent.Soundfile}
              </div>
              <DlgSndSelection
                label1="Sound Selection:"
                text=""
                className="form-control form-control-sm ml-2 mr-2"
                data-toggle="tooltip"
                data-placement="right"
                toolTip="Audiodatei wählen"
                modalID="Modal-S4"
                values={this.props.soundList}
                selection={this.setPropValue.bind(this, 'Soundfile')}
              />
            </div>

            <div className={uhr}>
              <div>
                Uhr :
              </div>

              <DropupTimer
                className="ml-2 mr-2"
                values={this.getTimerNames(this.props.seltimer ? this.props.seltimer.Name : null)}
                value={this.getDepTimerName(this.props.seltimerevent
                  ? this.props.seltimerevent.AndereTimerNr : null)}
                selection={this.setPropValue.bind(this, 'AndereTimerNr')}
              />
            </div>

            <div className={pagesel}>
              <div className="mr-1">
                Anzeigeseite :
              </div>

              <DropdownWithID
                direction="up"
                values={Pages}
                value={this.props.seltimerevent
                  ? parseInt(this.props.seltimerevent.Layer, 10) : null}
                wahl={this.setPropValue.bind(this, 'Page')}
              />
            </div>
          </div>
        </div>
      </div>
    );
  }
}

TimerEvents.propTypes = {
  timer: PropTypes.oneOfType(PropTypes.object),
  seltimer: PropTypes.oneOfType(PropTypes.object),
  timerevents: PropTypes.arrayOf(PropTypes.object),
  seltimerevent: PropTypes.oneOfType(PropTypes.object),
  soundList: PropTypes.oneOfType(PropTypes.object),
  pages: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func,
  timerEventSel: PropTypes.func,
};

TimerEvents.defaultProps = {
  timer: null,
  seltimer: null,
  timerevents: [],
  seltimerevent: null,
  soundList: [],
  websocketSend: () => {},
  timerEventSel: () => {},
};

export default TimerEvents;
