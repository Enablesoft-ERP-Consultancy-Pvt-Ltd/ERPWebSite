<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="2.0">
  <xsl:output method="html" indent="yes" />
  <xsl:template name="dots" match="/">
    <html xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <title>Emailer</title>
      </head>
      <body>
        <table width="700" border="1" align="center" cellpadding="0" cellspacing="0"  style="border: solid 1px #333">
          <xsl:for-each select="/Report/Inspection">
            <xsl:if test="position() = 1">

              <tr>
                <td  rowspan="3"
                    align="center" valign="middle" colspan="23">
                  <strong>
                    <xsl:value-of select="COMPANYNAME" />
                  </strong>
                </td>
                <td  align="left"
                    valign="middle">
                  <strong>
                    Doc. Name
                  </strong>
                </td>
                <td  align="left" valign="middle" colspan="6">

                  Undyed Yarn Inspection
                </td>
              </tr>




              <tr>
                <td align="left" valign="middle">
                  <strong>Doc. No.</strong>
                </td>
                <td colspan="6" align="left" valign="middle">
                  CHC-QC-Checklist     <xsl:value-of select="DOCID" />
                </td>
              </tr>
              <tr>
                <td align="left" valign="middle">
                  <strong>Date </strong>
                </td>
                <td colspan="6" align="left" valign="middle">

                  <xsl:value-of select="DATEADDED" />


                </td>
              </tr>
              <tr>
                <td colspan="23" rowspan="2" align="center" valign="middle" >
                  <strong>RAW YARN TESTING REPORT (AS PER AQL -2.5, SPECIAL LEVEL 1)</strong>
                </td>
                <td align="left" valign="middle" >
                  <strong>Version </strong>
                </td>
                <td colspan="6" align="left" valign="middle">03</td>
              </tr>
              <tr>
                <td align="left" valign="middle" >
                  <strong>Sys. Doc No </strong>
                </td>
                <td colspan="6" align="left" valign="middle">
                  <xsl:value-of select="DOCNO" />
                </td>
              </tr>

            </xsl:if>

          </xsl:for-each>

          <tr>
            <td  align="center" valign="middle" rowspan="3">Date</td>
            <td  align="center" valign="middle" rowspan="3">
              Supplier
              name
            </td>
            <td  align="center" valign="middle" rowspan="3">
              Challan
              No &amp; Date
            </td>

            <td  align="center" valign="middle" rowspan="3">
              Vender Lot No.
            </td>

            <td  align="center" valign="middle" rowspan="3">
              Int Lot No.
            </td>
            <td  align="center" valign="middle" rowspan="3">
              Yarn
              Type
            </td>
            <td  align="center" valign="middle" rowspan="3">
              Count
            </td>
            <td  align="center" valign="middle" rowspan="3">
              Total
              Bale
            </td>

            <td  align="center" valign="middle"    rowspan="3">

              Qty (kg)


            </td>
            <td  align="center" valign="middle" rowspan="3">
              Sample
              Size
            </td>
            <td  align="center" valign="middle" rowspan="3">
              No.
              of Hank
            </td>

            <td  align="center" valign="middle"  colspan="2"  rowspan="2">


              TPI
            </td>

            <td  align="center" valign="middle"  colspan="2"  rowspan="2">


              Count
            </td>
            <td  align="center" valign="middle"  colspan="2" rowspan="2">

              Fibere
              Length
            </td>
            <td  align="center" valign="middle"  colspan="4">

              Fibere
              Content
            </td>
            <td  align="center" valign="middle"  colspan="2" rowspan="2">

              Moisture
              Content
            </td>
            <td  align="center" valign="middle"  colspan="2" rowspan="2">

              pH
              Value
            </td>
            <td  align="center" valign="middle"  colspan="2" rowspan="2">
              Shade
            </td>
            <td  align="center" valign="middle" rowspan="3" >
              Lab
              Technician
            </td>
            <td  align="center" valign="middle"  rowspan="3">
              Result
            </td>
            <td  align="center" valign="middle"  rowspan="3">
              Comments
            </td>
          </tr>

          <tr>
            <td  align="center" valign="middle"  colspan="2">
              Pet

            </td>
            <td  align="center" valign="middle"  colspan="2">
              Others

            </td>

          </tr>

          <tr>
            <td  align="center" valign="middle" >
              Specification
            </td>
            <td  align="center" valign="middle" >
              Actual Avg.Value
            </td>

            <td  align="center" valign="middle" >
              Specification
            </td>
            <td  align="center" valign="middle" >
              Actual Avg.Value
            </td>


            <td  align="center" valign="middle" >
              Specification
            </td>
            <td  align="center" valign="middle" >
              Actual Avg.Value
            </td>



            <td  align="center" valign="middle" >
              Specification
            </td>
            <td  align="center" valign="middle" >
              Actual Avg.Value
            </td>

            <td  align="center" valign="middle" >
              Specification
            </td>
            <td  align="center" valign="middle" >
              Actual Avg.Value
            </td>

            <td  align="center" valign="middle" >
              Specification
            </td>
            <td  align="center" valign="middle" >
              Actual Avg.Value
            </td>


            <td  align="center" valign="middle" >
              Specification
            </td>
            <td  align="center" valign="middle" >
              Actual Avg.Value
            </td>


            <td  align="center" valign="middle" >
              Specification
            </td>
            <td  align="center" valign="middle" >
              Actual Avg.Value
            </td>

          </tr>
          <report>
            <xsl:for-each select="/Report/Inspection">
              <inspection>
                <tr>
                  <td  valign="top">
                    <xsl:value-of select="./REPORTDATE" />
                  </td>
                  <td  valign="top">
                    <xsl:value-of select="./SUPPLIERNAME" />
                  </td>
                  <td  align="left"
                      valign="middle" >

                    <xsl:value-of select="./CHALLANNO_DATE" />
                  </td>

                  <td  align="left"
                      valign="middle" >

                    <xsl:value-of select="./VenderLotNo" />
                  </td>

                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./LOTNO" />
                  </td>

                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./YARNTYPE" />
                  </td>
                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./COUNT" />
                  </td>
                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./TOTALBALE" />
                  </td>

                  <td  align="left"
                  valign="middle" >
                    -
                  </td>
                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./SAMPLESIZE" />
                  </td>
                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./NOOFHANK" />
                  </td>

                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./TPISpecification" />
                  </td>
                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./TPIAvgValue" />
                  </td>

                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./COUNTSpecification" />
                  </td>
                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./COUNTAvgValue" />
                  </td>

                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./FiberLengthSpecification" />
                  </td>
                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./FiberLengthAvgValue" />
                  </td>


                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./FiberPetSpecification" />
                  </td>
                  <td  align="left" valign="middle" >
                    <xsl:value-of select="./FiberPetAvgValue" />
                  </td>


                  <td  align="left" valign="middle">
                    <xsl:value-of select="./FiberOtherSpecification" />
                  </td>
                  <td  align="left" valign="middle">
                    <xsl:value-of select="./FiberOtherAvgValue" />
                  </td>


                  <td  align="left" valign="middle">
                    <xsl:value-of select="./MoistureSpecification" />
                  </td>
                  <td  align="left" valign="middle">
                    <xsl:value-of select="./MoistureAvgValue" />
                  </td>


                  <td  align="left" valign="middle">
                    <xsl:value-of select="./pHSpecification" />
                  </td>
                  <td  align="left" valign="middle">
                    <xsl:value-of select="./pHAvgValue" />
                  </td>


                  <td  align="left" valign="middle">
                    <xsl:value-of select="./ShadeSpecification" />
                  </td>
                  <td  align="left" valign="middle">
                    <xsl:value-of select="./ShadeAvgValue" />
                  </td>



                  <td  align="left" valign="middle">

                    <xsl:value-of select="./APPROVALUSERNAME" />
                  </td>
                  <td  align="left" valign="middle">
                    <xsl:value-of select="./STATUS" />
                  </td>
                  <td  align="left" valign="middle">
                    <xsl:value-of select="./COMMENTS" />
                  </td>
                </tr>

              </inspection>
            </xsl:for-each>
          </report>
          <tr>
            <td colspan="30" valign="middle" style=""></td>
          </tr>
          <tr>
            <td colspan="30" valign="middle" style=""></td>
          </tr>





        </table>

      </body>
    </html>




  </xsl:template>
</xsl:stylesheet>
