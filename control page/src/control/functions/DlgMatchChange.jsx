import React, { Component } from 'react';
import PropTypes from 'prop-types';
import CtrlEndMatch from '../../StdComponents/CtrlEndMatch';

class DlgMatchChange extends Component {
  constructor() {
    super();
    this.state = {
      SpielEndeOption: '1',
    };
  }

  CtrlEndMatchChange(e) {
    this.setState({ SpielEndeOption: e.target.value });
  }

  handleClick() {
    this.props.websocketSend({
      Domain: 'KO',
      Type: 'bef',
      Command: 'NextToActual',
      Value1: this.state.SpielEndeOption,
    });
  }

  render() {
    let disabled = true;
    if (this.props.turnierID !== 0) {
      disabled = false;
    }
    return (
      <div className="d-flex flex-column">
        <button
          type="button"
          className="btn btn-warning btn-sm border border-dark mt-1"
          data-placement="right"
          title="Spielendebehandlung"
          data-toggle="modal"
          data-target="#Spielendebehandlung"
          disabled={disabled}
        >
          aktives Spiel beenden / nächstes Spiel übernehmen
        </button>

        <div className="modal fade" id="Spielendebehandlung" tabIndex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
          <div className="modal-dialog modal-lg">
            <div className="modal-content">

              <div className="modal-header">
                <h5 className="modal-title text-dark">aktives Spiel beenden</h5>
                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">
                    &times;
                  </span>
                </button>
              </div>

              <div className="modal-body">
                <div className="d-flex flex-column">
                  <CtrlEndMatch
                    SpielEndeOption={this.state.SpielEndeOption}
                    CtrlEndMatchChange={this.CtrlEndMatchChange.bind(this)}
                  />
                </div>
              </div>

              <div className="modal-footer">
                <button
                  type="button"
                  className="btn btn-primary save"
                  onClick={this.handleClick.bind(this)}
                  data-dismiss="modal"
                >
                  anwenden
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

DlgMatchChange.propTypes = {
  websocketSend: PropTypes.func.isRequired,
  turnierID: PropTypes.number,
};

DlgMatchChange.defaultProps = {
  turnierID: 0,
};

export default DlgMatchChange;
