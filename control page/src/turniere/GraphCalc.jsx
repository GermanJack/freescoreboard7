import NodePair from './dataClasses/NodePair';

export default function GrafCalc(Turnier) {
  // Variablen init
  if (Turnier === null) {
    return '';
  }

  const np = [];
  const headline = Turnier.Kopf.Beschreibung;

  // Graph starten
  let sb = `digraph g \r\n{\r\n rankdir = "LR"; compound = "true"; nodesep = "0.5"; ranksep="1.25"; labelloc="top"; labeljust="left"; label = "${headline}";`;

  // subgraph je runde starten
  const anzrunden = Turnier.Runden.length;
  for (let i = 0; i < anzrunden; i += 1) {
    // Rundencluster immer aus Matrixdaten
    const r = Turnier.Runden[i];
    sb += `\r\nsubgraph cluster_${i} \r\n{\r\nlabel = "Runde ${r.Runde}\r\n${r.Rundenname}";`;

    // node je gruppe
    const gl1 = Turnier.Tabellen.filter((x) => x.Runde === r.Runde);
    const gl2 = gl1.map((x) => x.Gruppe);
    const gl = [...new Set(gl2)]; // distinct
    for (let y = 0; y < gl.length; y += 1) {
      // Gruppendaten immer aus Matrix
      const node = `"node${gl[y]}"`;
      sb += node;
      sb += '[label = "<f0> Gruppe ';
      sb += gl[y];

      // Mannschaft je gruppe
      let mlg = Turnier.Tabellen.filter((x) => x.Runde === r.Runde && x.Gruppe === gl[y]);

      // Prüfen ob Turnier mit bereits definierten Mannschaften
      if (Turnier.Tabellen !== null) {
        const mlgt = Turnier.Tabellen.filter((x) => x.Runde === r.Runde && x.Gruppe === gl[y]);
        if (mlgt.length > 0) {
          mlg = mlgt;
        }
      }

      for (let j = 0; j < mlg.length; j += 1) {
        const m = mlg[j];
        const f = `|<f${(j + 1)}>`;
        sb += f;
        if (m.Platz !== 0) {
          sb += `${m.Platz}. `;
        }
        // eslint-disable-next-line no-useless-escape
        sb += `${m.Mannschaft}`;

        // Pfeil Quelle und Ziel ermitteln
        if (mlg[j].Runde !== 1) {
          // von welcher Gruppe / Runde muss hierher gezeigt werden

          const sourceRunde = mlg[j].Quellrunde;
          if (sourceRunde === 0) {
            np.push(new NodePair(`"node${mlg[j].QuellGruppe}":<f0>`, `${node}${f.replace('|', ':')}`));
          } else {
            const sourceminus1 = (parseInt(sourceRunde, 10) - 1).toString();
            np.push(new NodePair(`"node${sourceRunde}A"`, `${node}${f.replace('|', ':')} [ltail = cluster_${sourceminus1}]`));
          }
        }
      }

      // node ende
      sb += '" shape = "record"];\r\n';
    }

    // subgraph ende
    sb += '}\r\n';
  }

  // Sieger / dritter
  const sp = Turnier.Spiele.filter((x) => x.SPlatz > 0);
  for (let i = 0; i < sp.length; i += 1) {
    const node = `"Platz ${sp[i].SPlatz}"`;
    sb += node;
    np.push(new NodePair(`"node${sp[i].Gruppe}":<f0>`, node));
  }

  // alle verbinder
  for (let i = 0; i < np.length; i += 1) {
    sb += `${np[i].Node1} -> ${np[i].Node2}\r\n`;
  }

  // graph ende
  sb += '}';

  // console.log(sb);
  return sb.toString();
}
