import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Seitenwahl extends Component {
  constructor(props) {
    super(props);
    this.Name = {
      value: null,
    };

    // This binding is necessary to make `this` work in the callback
    this.handleClick = this.handleClick.bind(this);
  }

  handleClick(e) {
    if (e === 'X') {
      this.props.websocketSend({ Type: 'bef', Command: 'ToogleDisplay' });
    } else {
      this.props.websocketSend({ Type: 'bef', Command: 'SwitchPage', Page: e });
    }
  }

  render() {
    let item = null;
    if (this.props.pages) {
      // eslint-disable-next-line arrow-body-style
      item = this.props.pages.map((i) => {
        return (
          <button
            type="button"
            className="btn btn-primary border-dark"
            key={i}
            data-dismiss="modal"
            onClick={this.handleClick.bind(this, i)}
          >
            {i}
          </button>
        );
      });
    }

    return (
      <div className="ml-2">

        <div className="d-flex flex-row">
          <div className="d-flex flex-col">
            <div className="mr-1 bottom-group-label">
              <div className="btn-group w-100">
                <span className="input-group-text border-dark bg-light" data-labelid="BoxSeitenwahl">Anzeigeseite:</span>
              </div>
            </div>
          </div>
          <div className="d-flex flex-col">
            <div className="btn-toolbar">
              <div className="btn-group mb-1">
                {item}
                <button
                  type="button"
                  className="btn btn-primary border-dark"
                  data-labelid="BtnX"
                  onClick={this.handleClick.bind(this, 'X')}
                  title="lokale Anzeige ein/aus"
                >
                  X
                </button>
              </div>
            </div>
          </div>
        </div>

      </div>
    );
  }
}

Seitenwahl.propTypes = {
  pages: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default Seitenwahl;
