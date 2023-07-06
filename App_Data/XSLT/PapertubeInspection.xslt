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
                    align="center" valign="middle" colspan="12">
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
                  PAPER TUBE TESTING REPORT
                </td>
              </tr>
              <tr>
                <td align="left" valign="middle">
                  <strong>Doc. No.</strong>
                </td>
                <td colspan="5" align="left" valign="middle">
                  <xsl:value-of select="DOCNO" />
                </td>
              </tr>
              <tr>
                <td align="left" valign="middle">
                  <strong>Eff. Date. </strong>
                </td>
                <td colspan="5" align="left" valign="middle">

                  <xsl:value-of select="ADDEDDATE" />


                </td>
              </tr>
              <tr>
                <td colspan="12" rowspan="2" align="center" valign="middle">
                  <strong>
                    PAPER TUBE TESTING REPORT( Inspection Level - AQL-2.5,Special Level-1)



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
            <td align="center" valign="middle">Date</td>
            <td align="center" valign="middle">
              Supplier name
            </td>
            <td align="center" valign="middle">
              Challan No &amp; Date
            </td>
            <td align="center" valign="middle">
              Sr. No.
            </td>

            <td align="center" valign="middle">
              Description
            </td>
            <td align="center" valign="middle">
              Total Qty.
            </td>
            <td align="center" valign="middle">
              Sample Size
            </td>
            <td align="center" valign="middle">
              Length
            </td>
            <td align="center" valign="middle">
              Thickness
            </td>

            <td align="center" valign="middle">
              Inner Dia
            </td>
            <td align="center" valign="middle">
              Moisture
            </td>
            <td align="center" valign="middle">
              Weight
            </td>
            <td align="center" valign="middle">
              Found
            </td>
            <td align="center" valign="middle">
              Acceptance
            </td>
            <td align="center" valign="middle">
              Comments
            </td>
            <td align="center" valign="middle">
              Lot Result
            </td>
            <td align="center" valign="middle">
              Lab Technician
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
                    <xsl:value-of select="./SupplierName" />
                  </td>
                  <td align="left"
                      valign="middle">

                    <xsl:value-of select="./ChallanNoDate" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./Srno" />
                  </td>
                  <td align="left"
                      valign="middle">

                    <xsl:value-of select="./Description" />
                  </td>

                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./totalqty" />
                  </td>

                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./samplesize" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./Length" />
                  </td>

                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./Thickness" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./InnerDia" />
                  </td>

                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./Moisture" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./Weight" />
                  </td>

                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./Found" />
                  </td>


                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./Acceptance" />
                  </td>
                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./COMMENTS" />
                  </td>



                  <td align="left"
                      valign="middle">
                    <xsl:value-of select="./Lotresult" />
                  </td>

                  <td align="left" valign="middle">
                    <xsl:value-of select="./APPROVALUSERNAME" />
                  </td>
                </tr>
              </inspection>
            </xsl:for-each>
          </report>

          <tr>
            <td colspan="17" valign="middle"></td>
          </tr>
          <tr>
            <td colspan="17" valign="middle"></td>
          </tr>
        </table>

      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
