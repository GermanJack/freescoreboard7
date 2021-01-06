import React, { Component } from 'react';
import PropTypes from 'prop-types';

class AddFenster extends Component {
  constructor() {
    super();
    this.state = {
      newFenster: null,
    };
  }

  handleSubmit(e) {
    // console.log(this.refs.titel.value);

    this.setState(
      {
        newFenster: {
          titel: this.refs.titel.value,
          spalte: this.refs.spalte.value
        },
      }, function x() {
        this.props.addFenster(this.state.newFenster);
      },
    );
    e.preventDefault();
  }

  render() {
    // eslint-disable-next-line arrow-body-style
    const Titelliste = this.props.Titel.map((titel) => {
      return (
        <option key={titel} value={titel}>
          {titel}
        </option>
      );
    });

    // eslint-disable-next-line arrow-body-style
    const Spaltenliste = this.props.Spalte.map((titel) => {
      return (
        <option key={titel} value={titel}>
          {titel}
        </option>
      );
    });

    return (
      <div className="border border-dark m-1">
        <form onSubmit={this.handleSubmit.bind(this)}>
          <div className="form-group">
            <div className="">
              <select ref="spalte">
                {Spaltenliste}
              </select>
              <select ref="titel">
                {Titelliste}
              </select>

              <input type="submit" value="+" />
            </div>
          </div>
        </form>
      </div>
    );
  }
}

AddFenster.propTypes = {
  Titel: PropTypes.arrayOf(PropTypes.string),
  Spalte: PropTypes.arrayOf(PropTypes.string),
  addFenster: PropTypes.func.isRequired,
};

AddFenster.defaultProps = {
  Titel: ['Mannschaften', 'Tore', 'Foul', 'Zeiten'],
  Spalte: ['1', '2', '3'],
};

export default AddFenster;
