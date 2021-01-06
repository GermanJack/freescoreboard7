import React, { Component } from 'react';
import PropTypes from 'prop-types';
import $ from 'jquery';
import { GoPlus } from 'react-icons/go';
import DropdownWithID from './DropdownWithID';

class DlgPenalty extends Component {
  constructor(props) {
    super(props);
    this.state = {
      player: '',
      penalty: '',
      closemod: '',
    };
  }

  componentDidMount() {
    $(`#${this.props.modalID}`).on('show.bs.modal', () => {
      this.onModalShow();
    });

    $(`#${this.props.modalID}`).on('hidden.bs.modal', () => {
      this.onModalClose();
    });
  }

  onModalShow() {
    this.setState({ closemod: '' });
  }

  onModalClose() {
    // how is modal closed
    // console.log(this.state.closemod);
    if (this.state.closemod === 'ok') {
      if (this.state.player !== '' && this.state.penalty !== '') {
        this.props.newPenalty({ playerid: this.state.player, penaltyid: this.state.penalty });
      }
    }
  }

  handleClick() {
    this.setState({ closemod: 'ok' });
  }

  penaltyChange(e) {
    this.setState({ penalty: e });
  }

  playerChange(e) {
    this.setState({ player: e });
  }

  render() {
    let pll = [];
    if (this.props.playerlist) {
      pll = this.props.playerlist.map((x) => ({ ID: x.ID, Text: (`${x.Nachname}, ${x.Vorname}`) }));
    }

    let pel = [];
    if (this.props.penalties) {
      pel = this.props.penalties.map((x) => ({ ID: x.ID, Text: x.Bezeichnung }));
    }

    return (
      <div>
        <button
          type="button"
          className="btn btn-primary border-dark ml-1 p-0"
          data-placement="right"
          data-toggle="modal"
          data-target={`#${this.props.modalID}`}
        >
          <GoPlus size="1.5em" color="white" />
        </button>

        <div className="modal fade" id={this.props.modalID} tabIndex="-1" role="dialog" aria-labelledby={this.props.modalID} aria-hidden="true">
          <div className="modal-dialog modal-sm modal-dialog-centered" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title text-dark">{this.props.label1}</h5>
                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div className="modal-body p-1">

                <table>
                  <tr>
                    <td className="">Spieler:</td>
                    <td className="col-sm-4">
                      <DropdownWithID
                        values={pll}
                        value={this.state.player}
                        wahl={this.playerChange.bind(this)}
                      />
                    </td>
                  </tr>

                  <tr>
                    <td className="">Strafe:</td>
                    <td className="col-sm-4">
                      <DropdownWithID
                        values={pel}
                        value={this.state.penalty}
                        wahl={this.penaltyChange.bind(this)}
                      />
                    </td>
                  </tr>
                </table>

              </div>

              <div className="modal-footer">
                <div className="text-dark">{this.state.message}</div>
                <button type="button" className="btn btn-primary" data-dismiss="modal" onClick={this.handleClick.bind(this)}>OK</button>
              </div>

            </div>
          </div>
        </div>
      </div>
    );
  }
}

DlgPenalty.propTypes = {
  modalID: PropTypes.string,
  label1: PropTypes.string,
  playerlist: PropTypes.arrayOf(PropTypes.object),
  penalties: PropTypes.arrayOf(PropTypes.object),
  newPenalty: PropTypes.func,
};

DlgPenalty.defaultProps = {
  modalID: 'filedlg',
  label1: 'neue Strafe',
  playerlist: [],
  penalties: [],
  newPenalty: () => {},
};

export default DlgPenalty;
