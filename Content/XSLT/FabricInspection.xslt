<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="2.0">
  <xsl:output method="html" indent="yes" />
  <xsl:template name="dots" match="/">
    <html xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <title>Emailer</title>
      </head>
      <body>
        <table width="700" border="1" align="center" cellpadding="0" cellspacing="0" style="border: solid 1px #333">
          <xsl:for-each select="/Report/Inspection">
            <xsl:if test="position() = 1">

              <tr>
                <td rowspan="3"
                    align="center" valign="middle" colspan="19">
                  <strong>

                    <xsl:value-of select="COMPANYNAME" />
                  </strong>
                </td>
                <td align="left"
                    valign="middle">
                  <strong>
                    Doc. Name
                  </strong>
                </td>
                <td align="left" valign="middle" colspan="5">
                  DUCK COTTON CLOTH TGT
                </td>
              </tr>




              <tr>
                <td align="left" valign="middle">
                  <strong>Doc. No.</strong>
                </td>
                <td colspan="5" align="left" valign="middle">
                  CHC-QC-Checklist
                  <xsl:value-of select="Docid" />
                </td>
              </tr>
              <tr>
                <td align="left" valign="middle">
                  <strong>Date </strong>
                </td>
                <td colspan="5" align="left" valign="middle">

                  <xsl:value-of select="ADDEDDATE" />


                </td>
              </tr>
              <tr>
                <td colspan="19" rowspan="2" align="center" valign="middle">
                  <strong>
                    FABRIC TESTING REPORT ( AS PER AQL-2.5,SPECIAL LEVEL -1)



                  </strong>
                </td>
                <td align="left" valign="middle">
                  <strong>Version </strong>
                </td>
                <td colspan="5" align="left" valign="middle">03</td>
              </tr>
              <tr>
                <td align="left" valign="middle">
                  <strong>Sys. Doc No </strong>
                </td>
                <td colspan="5" align="left" valign="middle">
                  <xsl:value-of select="DocNo" />
                </td>
              </tr>

            </xsl:if>

          </xsl:for-each>

          <tr>
            <td align="center" valign="middle" rowspan="3">Date</td>
            <td align="center" valign="middle" rowspan="3">
              Supplier
              name
            </td>
            <td align="center" valign="middle" rowspan="3">
              Challan
              No &amp; Date
            </td>

            <td align="center" valign="middle" rowspan="3">
              No. of Roll
            </td>

            <td align="center" valign="middle" rowspan="3">
              Total Mtr
            </td>
            <td align="center" valign="middle" rowspan="3">
              Total Mtr(Insp.)

            </td>
            <td align="center" valign="middle" rowspan="2" colspan="2">
              PPI
            </td>
            <td align="center" valign="middle" rowspan="2" colspan="2">
              EPI

            </td>
            <td align="center" valign="middle" rowspan="2" colspan="2">
              GSM

            </td>

            <td align="center" valign="middle" colspan="4">
              Fiber Content

            </td>
            <td align="center" valign="middle" rowspan="2" colspan="2">
              Colour
            </td>

            <td align="center" valign="middle" rowspan="2" colspan="2">
              Width


            </td>

            <td align="center" valign="middle" rowspan="2" colspan="2">
              Moisture

            </td>


            <td align="center" valign="middle" rowspan="3">
              Comments


            </td>
            <td align="center" valign="middle" rowspan="3">
              Lot Result

            </td>
            <td align="center" valign="middle" rowspan="3">
              Lab
              Technician
            </td>

          </tr>

          <tr>
            <td align="center" valign="middle" colspan="2">Cotton </td>
            <td align="center" valign="middle" colspan="2">Other </td>
          </tr>

          <tr>
            <td align="center" valign="middle">
              spec.


            </td>
            <td align="center" valign="middle">
              Avg.


            </td>
            <td align="center" valign="middle">
              spec.


            </td>
            <td align="center" valign="middle">
              Avg.


            </td>
            <td align="center" valign="middle">
              spec.


            </td>
            <td align="center" valign="middle">
              Avg.


            </td>
            <td align="center" valign="middle">
              spec.


            </td>
            <td align="center" valign="middle">
              Avg.


            </td>
            <td align="center" valign="middle">
              spec.


            </td>
            <td align="center" valign="middle">
              Avg.


            </td>
            <td align="center" valign="middle">
              spec.


            </td>
            <td align="center" valign="middle">
              Avg.


            </td>
            <td align="center" valign="middle">
              spec.


            </td>
            <td align="center" valign="middle">
              Avg.


            </td>
            <td align="center" valign="middle">
              spec.


            </td>
            <td align="center" valign="middle">
              Avg.


            </td>



          </tr>
          <report>
            <xsl:for-each select="/Report/Inspection">
              <inspection>
                <tr>
                  <td valign="top">
                    <xsl:value-of select="./REPORTDATE" />
                  </td>
                  <td valign="top">
                    <xsl:value-of select="./SUPPLIERNAME" />
                  </td>
                  <td align="left"
                      valign="middle">

                    <xsl:value-of select="./CHALLANNO" />
                  </td>

                  <td align="left"
                      valign="middle">

                    <xsl:value-of select="./NOOFROLL" />
                  </td>

                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./TOTALMETER" />
                  </td>

                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./TOTALMETERINSPECTED" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./SPECIFICATION_1" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./AVGVALUE_1" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./SPECIFICATION_2" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./AVGVALUE_2" />
                  </td>


                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./SPECIFICATION_3" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./AVGVALUE_3" />
                  </td>



                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./SPECIFICATIONPET_4" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./AVGVALUEPET_4" />
                  </td>



                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./SPECIFICATIONOTHER_4" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./AVGVALUEOTHER_4" />
                  </td>


                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./SPECIFICATION_5" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./AVGVALUE_5" />
                  </td>


                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./SPECIFICATION_6" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./AVGVALUE_6" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./SPECIFICATION_7" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./AVGVALUE_7" />
                  </td>

                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./COMMENTS" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./STATUS" />
                  </td>
                  <td align="left" valign="middle">
                    <xsl:value-of select="./APPROVALUSERNAME" />
                  </td>
                </tr>
              </inspection>
            </xsl:for-each>
          </report>

          <tr>
            <td colspan="25" valign="middle" style=""></td>
          </tr>
          <tr>
            <td colspan="25" valign="middle" style=""></td>
          </tr>





        </table>

      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>