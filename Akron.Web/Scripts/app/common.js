function print_filter(filter) {
    var f = eval(filter);
    if (typeof (f.length) != "undefined") { } else { }
    if (typeof (f.top) != "undefined") { f = f.top(Infinity); } else { }
    if (typeof (f.dimension) != "undefined") { f = f.dimension(function (d) { return ""; }).top(Infinity); } else { }
    console.log(filter + "(" + f.length + ") = " + JSON.stringify(f).replace("[", "[\n\t").replace(/}\,/g, "},\n\t").replace("]", "\n]"));
}

var myColors = ['#99d8c9', '#66c2a4', '#41ae76', '#238b45', '#006d2c', '#00441b', '#bfd3e6', '#9ebcda', '#8c96c6', '#8c6bb1', '#88419d', '#810f7c', '#4d004b', '#a8ddb5', '#7bccc4', '#4eb3d3', '#2b8cbe', '#0868ac', '#084081', '#fdbb84', '#fc8d59', '#ef6548', '#d7301f', '#b30000', '#7f0000', '#d4b9da', '#c994c7', '#df65b0', '#e7298a', '#ce1256', '#980043', '#67001f', '#fcc5c0', '#fa9fb5', '#f768a1', '#dd3497', '#ae017e', '#7a0177', '#49006a', '#addd8e', '#78c679', '#41ab5d', '#238443', '#006837', '#004529', '#c7e9b4', '#7fcdbb', '#41b6c4', '#1d91c0', '#225ea8', '#253494', '#081d58', '#fee391', '#fec44f', '#fe9929', '#ec7014', '#cc4c02', '#993404', '#662506', '#fed976', '#feb24c', '#fd8d3c', '#fc4e2a', '#e31a1c', '#bd0026', '#800026'];