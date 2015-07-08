var data = null;
var ndx = null;
var mainDim = null;
var mainGroup = null;
var chart = dc.seriesChart("#chart");
var barChart = dc.barChart("#barChart");
var bubbleChart = dc.bubbleChart('#bubbleChart');
var actionBtn = Ladda.create(document.querySelector('button[data-action="actions"]'));
var yearDim = null;
var yearGroup = null;
var ordinals;
var countNdx = null;
var countDim = null;
var countDimGroup = null;
var colors = null;
var mainGroupAll;
var yearGroupAll = null;
var countGroupAll = null;

function init(dimensionType) {

    d3.json(root + 'api/RJI/' + dimensionType, function (r) {
        data = r;
        $('#recordCount').text(r.RecordCount);
        $('#averageBasePay').text(r.TotalAverage);
        $('#totalsTable tbody').empty();
        var dimTypes = [];
        data.BasePayByYearAndDimension.forEach(function (d) {
            var org = d[0]._value[1]._value;
            if (dimTypes.indexOf(org) < 0)
                dimTypes.push(org);
        });
        $('#dimCount').text(dimTypes.length);
        $('#dimLabel').text(r.DimensionLabel);

        ndx = crossfilter(data.BasePayByYearAndDimension);
        countNdx = crossfilter(data.CountByDimension);

        mainDim = ndx.dimension(function (d) {
            return [d[0]._value[0]._value, d[0]._value[1]._value];
        });
        yearDim = ndx.dimension(function (d) {
            return d[0]._value[0]._value;
        });

        countDim = countNdx.dimension(function (d) {
            return d[0]._value;
        });
        mainGroup = mainDim.group().reduceSum(function (d) {
            return d[1]._value;
        });
        countDimGroup = countDim.group().reduce(function (p, v) {
            p.count = v[1]._value;
            return p;
        }, function (p, v) {

        }, function () {
            return { count: 0 };
        });
        yearGroup = yearDim.group().reduce(function (p, v) {
            ++p.count;
            p.total += v[1]._value;
            p.average = Math.floor(p.total / p.count);
            return p;
        }, function (p, v) {

        }, function () {
            return { count: 0, average: 0, total: 0 };
        });

        mainGroupAll = mainGroup.all();
        yearGroupAll = yearGroup.all();


        var chartDomainMin = d3.min(mainGroupAll, function (d) { return d.value - (d.value * .20); });
        var chartDomainMax = d3.max(mainGroupAll, function (d) { return d.value; });
        var yearDomainMin = d3.min(yearGroupAll, function (d) { return d.key; });
        var yearDomainMax = d3.max(yearGroupAll, function (d) { return d.key; });
        var barDomainMinY = d3.min(yearGroupAll, function (d) { return d.value.average - (d.value.average * .10); });
        var barDomainMaxY = d3.max(yearGroupAll, function (d) { return d.value.average; });
        $('#yearCount').text(yearGroup.all().length);


        chart
            .dimension(mainDim)
            .width(1200)
            .height(400)
            .group(mainGroup)
            .colors(d3.scale.ordinal().range(myColors))
            .brushOn(false)
            .legend(dc.legend().x(650).y(20).itemHeight(8).gap(5).autoItemWidth(true))
            .y(d3.scale.linear().domain([chartDomainMin, chartDomainMax]))
            .x(d3.scale.linear().domain([yearDomainMin, yearDomainMax]))
            .keyAccessor(function (d) {
                return d.key[0];
            })
            .valueAccessor(function (d) {
                return d.value;
            })
            .seriesAccessor(function (d) {
                return d.key[1];
            });

        chart.yAxis().tickFormat(function (v) {
            return v / 1000;
        });
        chart.xAxis().ticks(7);
        chart.xAxis().tickFormat(function (v) {
            return v;
        });
        chart.yAxis().tickFormat(function (v) {
            return v / 1000 + 'K';
        });
        chart.margins().right = 600;
        chart.margins().left = 50;
        barChart
            .width(300)
            .height(200)
            .x(d3.scale.linear().domain([yearDomainMin, yearDomainMax]))
            .y(d3.scale.linear().domain([barDomainMinY, barDomainMaxY]))
            .gap(1)
            .brushOn(false)
            .dimension(yearDim)
            .group(yearGroup)
            .keyAccessor(function (d) {
                return d.key;
            })
            .valueAccessor(function (d) {
                return d.value.average;
            });
        barChart.yAxis().tickFormat(function (v) {
            return v / 1000 + 'K';
        });
        barChart.xAxis().tickFormat(function (d) {
            return d.toString();
        });
        barChart.xAxis().ticks(7);
        var ordinals = [];
        var all = countDimGroup.all();
        all.forEach(function (d) {
            ordinals.push(d.key);
        });
        var totalCount = d3.sum(all, function (d) { return d.value.count; });
        var maxCount = d3.max(all, function (d) { return d.value.count; });
        var maxDomain = ((maxCount / totalCount) * 150);
        bubbleChart
        .width(1200)
                    .height(250)
                    .transitionDuration(1500)
                    .margins({ top: 30, right: 50, bottom: 30, left: 40 })
                    .dimension(countDim)

                    .group(countDimGroup)
                    .colorDomain([-0.0, 100.1])
                    .colors(d3.scale.category20b())
                    .yAxisPadding(20)
                    .colorDomain([0, 100])
                    .colorAccessor(function (d) {
                        return Math.round((d.value.count / totalCount) * 100);
                    })
                    .keyAccessor(function (p) {
                        return p.key;
                    })
                    .valueAccessor(function (p) {
                        return Math.round(((p.value.count / totalCount) * 100), 4);
                    })
                    .radiusValueAccessor(function (p) {
                        return (p.value.count / totalCount) * 40;
                    })
                    .maxBubbleRelativeSize(0.5)
                    .x(d3.scale.ordinal().domain(ordinals))
                    .xUnits(dc.units.ordinal)
                    .y(d3.scale.linear().domain([-5, maxDomain]))
                    .r(d3.scale.linear().domain([0, 100]))
                    .xAxisPadding(900)
                    .title(function (d) {
                        return d.key + ':' + Math.round(((d.value.count / totalCount) * 100), 2) + '%';
                    })
                    .elasticX(true)

                    .yAxisPadding(100)
                    .xAxisPadding(500)
                    .renderHorizontalGridLines(true)
                    .renderVerticalGridLines(true)
                    .label(function (d) {
                        var lbl = d.key;
                        if (d.key.length > 12)
                            lbl = d.key.substring(0, 10) + "...";
                        return lbl;
                    });

        bubbleChart.yAxis().tickFormat(function (v) {
            return v + '%';
        });

        yearGroup.all().forEach(function (val) {
            var val = $('<tr><td>' + val.key + '</td><td>' + val.value.average + '</td></tr>');

            $('#totalsTable tbody').append(val);
        });

        dc.renderAll();
        actionBtn.stop();
    });
}

$(function () {
    

    init('JobFamily');

    $(document).on('click', '.dimensionSelect', function (e) {
        actionBtn.start();
        var dimension = $(this).data('dimension');
        init(dimension);
    });
});