import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { MdFormatAlignLeft, MdFormatAlignCenter, MdFormatAlignRight } from 'react-icons/md';

class DropdownHAlign extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  handleClick(e) {
    this.props.onClick(e);
  }

  calcAlignIcon(h) {
    if (h === 'flex-start') {
      return (<MdFormatAlignLeft />);
    }

    if (h === 'center') {
      return (<MdFormatAlignCenter />);
    }

    if (h === 'flex-end') {
      return (<MdFormatAlignRight />);
    }

    return '';
  }

  render() {
    const aicon = this.calcAlignIcon(this.props.halign);
    return (
      <div className="">
        <div className="dropdown">
          <button
            className="btn btn-outline-secondary btn-icon p-0 pl-1 pr-1 pb-1 dropdown-toggle"
            type="button"
            data-placement="right"
            title="horizontale Ausrichtung"
            data-toggle="dropdown"
            aria-haspopup="true"
            aria-expanded="false"
          >
            {aicon}
          </button>
          <div className="dropdown-menu p-0">

            <div className="d-flex flex-row">
              <button
                type="button"
                value="flex-start"
                className="btn btn-outline-secondfary p-0 pl-1 pr-1"
                onClick={this.handleClick.bind(this, 'flex-start')}
                title="links"
              >
                <div className="d-flex flex-row align-items-center">
                  <MdFormatAlignLeft />
                  <div className="ml-1">links</div>
                </div>
              </button>
            </div>
            <div className="d-flex flex-row">
              <button
                type="button"
                value="center"
                className="btn btn-outline-secondfary p-0 pl-1 pr-1"
                onClick={this.handleClick.bind(this, 'center')}
                title="zentriert"
              >
                <div className="d-flex flex-row align-items-center">
                  <MdFormatAlignCenter />
                  <div className="ml-1">zentriert</div>
                </div>
              </button>
            </div>
            <div className="d-flex flex-row">
              <button
                type="button"
                value="flex-end"
                className="btn btn-outline-secondfary p-0 pl-1 pr-1"
                onClick={this.handleClick.bind(this, 'flex-end')}
                title="rechts"
              >
                <div className="d-flex flex-row align-items-center">
                  <MdFormatAlignRight />
                  <div className="ml-1">rechts</div>
                </div>
              </button>
            </div>

          </div>
        </div>
      </div>
    );
  }
}

DropdownHAlign.propTypes = {
  halign: PropTypes.string,
  onClick: PropTypes.func,
};

DropdownHAlign.defaultProps = {
  halign: '',
  onClick: () => {},
};

export default DropdownHAlign;
