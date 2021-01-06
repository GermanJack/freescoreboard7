import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Position from './ObjectProperties/Position';
import Size from './ObjectProperties/Size';
import Background from './ObjectProperties/Background';
import Content from './ObjectProperties/Content';
import Font from './ObjectProperties/Font';
import Border from './ObjectProperties/Border';
import TableSetup from './ObjectProperties/TableSetup';

class ObjectProperties extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  handleStyleChange(style, value) {
    if (this.props.objekt) {
      this.props.ObjektStyleChange(style, value);
    }
  }

  handleAttributChange(Attribute, Value) {
    if (this.props.objekt) {
      this.props.ObjektAttributeChange(Attribute, Value);
    }
  }

  render() {
    // const oid = this.props.objekt ? this.props.objekt.getAttribute('data-oid') : null;
    let s = '';
    if (this.props.objekt) {
      s = JSON.parse(this.props.objekt.style);
    }

    return (
      <div className="d-flex flex-column border border-dark mh-25">
        <div className="d-flex flex-row border border-dark bg-secondary text-light pl-2">
          Objekteigenschaften
          {this.props.objekt ? ` ID: ${this.props.objekt.ID}` : null}
        </div>

        <div className="d-flex flex-row border border-dark">

          <Position
            top={s.top ? s.top : 0}
            left={s.left ? s.left : 0}
            onStyleChange={this.handleStyleChange.bind(this)}
          />

          <Size
            height={s.height ? s.height : 0}
            width={s.width ? s.width : 0}
            visibility={s.visibility ? s.visibility : 'visible'}
            onStyleChange={this.handleStyleChange.bind(this)}
          />

          <Background
            picList={this.props.picList}
            color={s['background-color'] ? s['background-color'] : '#ffffffff'}
            bildMod={s['background-size'] ? s['background-size'] : 'contain'}
            onAtributeChange={this.handleAttributChange.bind(this)}

            picVariables={this.props.picVariables}
            bildvariable={this.props.objekt ? this.props.objekt.bgid : null}

            onStyleChange={this.handleStyleChange.bind(this)}
          />

          <Content
            textVariables={this.props.textVariables}
            variable={this.props.objekt ? this.props.objekt.textid : null}
            text={this.props.objekt ? this.props.objekt.innerText : ''}
            speed={this.props.objekt ? this.props.objekt.Speed : 0}
            onAtributeChange={this.handleAttributChange.bind(this)}

            ha={s['justify-content'] ? s['justify-content'] : 'center'}
            va={s['align-items'] ? s['align-items'] : 'center'}
            onStyleChange={this.handleStyleChange.bind(this)}
          />

          <Font
            fontList={this.props.fontList}
            font={s['font-family'] ? s['font-family'] : 'Arial'}
            size={s['font-size'] ? s['font-size'] : '5vh'}
            color={s.color ? s.color : '#ffffffff'}
            style={s['font-style'] ? s['font-style'] : 'normal'}
            weight={s['font-weight'] ? s['font-weight'] : 'normal'}
            onStyleChange={this.handleStyleChange.bind(this)}
          />

          <Border
            color={s['border-color'] ? s['border-color'] : '#ffffffff'}
            dicke={s['border-width'] ? s['border-width'] : '0px'}
            radius={s['border-radius'] ? s['border-radius'] : '0vh'}
            onStyleChange={this.handleStyleChange.bind(this)}
          />

          <TableSetup
            tableVariables={this.props.tableVariables}
            tablevariable={this.props.objekt ? this.props.objekt.tableid : null}
            tablestyle={this.props.objekt ? this.props.objekt.TableStyle : 'Standard'}
            onAtributeChange={this.handleAttributChange.bind(this)}
          />

        </div>

      </div>
    );
  }
}

ObjectProperties.propTypes = {
  picVariables: PropTypes.arrayOf(PropTypes.object).isRequired,
  objekt: PropTypes.oneOfType(PropTypes.object).isRequired,
  picList: PropTypes.arrayOf(PropTypes.object).isRequired,
  fontList: PropTypes.arrayOf(PropTypes.string).isRequired,
  textVariables: PropTypes.arrayOf(PropTypes.string).isRequired,
  tableVariables: PropTypes.arrayOf(PropTypes.string).isRequired,
  ObjektStyleChange: PropTypes.func.isRequired,
  ObjektAttributeChange: PropTypes.func.isRequired,
};

export default ObjectProperties;
