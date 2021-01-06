import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Table from '../../StdComponents/Table';

class Events extends Component {
  constructor(props) {
    super(props);
    this.state = {
      selevent: '',
    };
  }

  componentDidMount() {
    if (!this.state.selevent) {
      this.setState({ selevent: this.props.events[0] });
    }
  }

  setEvent(ID) {
    if (ID) {
      this.props.websocketSend({ Command: 'SetEvent', Value1: ID, Value2: !this.state.selevent.Log });
      // eslint-disable-next-line react/no-access-state-in-setstate
      const se = this.state.selevent;

      se.Log = !this.state.selevent.Log;
      this.setState({ selevent: se });
    }
  }

  rowClick(ID) {
    this.setState({ selevent: this.props.events.find((x) => x.ID === ID) });
  }

  delFreeEreig() {
    this.props.websocketSend({ Command: 'delFreeEreig' });
  }

  render() {
    return (
      <div className="p-1 ml-2">
        <u>Ereignisprotokoll</u>

        <div className="p-1 ml-2">
          <button type="button" onClick={this.delFreeEreig.bind(this)}>Freispielereignisse löschen</button>
        </div>

        <div className="d-flex flex-row p-1 ml-2">

          <Table
            daten={this.props.events}
            cols={[
              { Column: 'Nummer', Label: 'Ereignis' },
              { Column: 'Log', Label: 'Protokoll aktiv' },
            ]}
            chk="Log"
            chkid={[this.state.selevent.ID]}
            radiogrp="timerevents"
            rowClick={this.rowClick.bind(this)}
            // evtType={this.state.evtType}
          />

          <div className="d-flex flex-column p-1 ml-2">
            <div>
              {this.state.selevent ? this.state.selevent.Nummer : 'kein Ereignis gewählt'}
            </div>

            <div>
              <label htmlFor="log" className="switch pl-2 ml-1 text-center">
                <input
                  type="checkbox"
                  id="log"
                  checked={this.state.selevent ? this.state.selevent.Log : null}
                  onChange={this.setEvent.bind(this, this.state.selevent.ID)}
                />
                <span className="slider round" />
              </label>
            </div>
          </div>


        </div>
      </div>
    );
  }
}

Events.propTypes = {
  events: PropTypes.oneOfType(PropTypes.object),
  websocketSend: PropTypes.func,
};

Events.defaultProps = {
  events: [],
  websocketSend: () => {},
};

export default Events;
