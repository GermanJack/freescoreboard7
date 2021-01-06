import React from 'react';
import PropTypes from 'prop-types';
import reactCSS from 'reactcss';
import { SketchPicker } from 'react-color';

class DlgColorPicker extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      displayColorPicker: false,
    };

    this.handleClick = this.handleClick.bind(this);
    this.handleChange = this.handleChange.bind(this);
  }

  handleClick() {
    this.setState((prevState) => ({
      displayColorPicker: !prevState.displayColorPicker,
    }));
  }

  handleChange(color) {
    this.upliftChange(color.rgb);
  }

  upliftChange(color) {
    const c = `rgba(${color.r}, ${color.g}, ${color.b}, ${color.a})`;
    this.props.onChange(c);
  }

  render() {
    const styles = reactCSS({
      default: {
        color: {
          width: '20px',
          height: '20px',
          borderRadius: '2px',
          background: this.props.color,
        },
        popover: {
          position: 'absolute',
          zIndex: '999',
        },
        cover: {
          position: 'fixed',
          top: '0px',
          right: '0px',
          bottom: '0px',
          left: '0px',
        },
        butten: {
          backgroundImage: 'radial-gradient(black 5%, white 50%, black 95%);',
        },
      },
    });

    return (
      <div>
        <div
          className="btn btn-outline-secondary btn-icon p-1 pl-1 pr-1 pb-1"
          style={{ backgroundImage: 'radial-gradient(black 5%, white 50%, black 95%);' }}
          data-placement="right"
          title={this.props.toolTip}
          onClick={this.handleClick}
          role="button"
          tabIndex="0"
        >
          <div className="border" style={styles.color} />
        </div>

        { this.state.displayColorPicker ? (
          <div style={styles.popover}>
            <div
              role="button"
              tabIndex={-1}
              style={styles.cover}
              onClick={this.handleClick}
              label="color picker"
            />
            <SketchPicker
              value={this.props.color}
              color={this.props.color}
              onChangeComplete={this.handleChange}
              disableAlpha={false}
              presetColors={['#D0021B', '#F5A623', '#F8E71C', '#8B572A', '#7ED321', '#417505', '#BD10E0', '#9013FE', '#4A90E2', '#50E3C2', '#B8E986', '#000000', '#4A4A4A', '#9B9B9B', '#ffffff', 'transparent']}
              width={200}
            />
          </div>
        ) : null }

      </div>
    );
  }
}

DlgColorPicker.propTypes = {
  color: PropTypes.string,
  toolTip: PropTypes.string,
  onChange: PropTypes.func,
};

DlgColorPicker.defaultProps = {
  color: '#000000',
  toolTip: 'color picker',
  onChange: () => {},
};

export default DlgColorPicker;
