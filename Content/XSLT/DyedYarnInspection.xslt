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
                    align="center" valign="middle" colspan="13">
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
                <td  align="left" valign="middle" colspan="5">

                  Undyed Yarn Inspection
                </td>
              </tr>




              <tr>
                <td align="left" valign="middle">
                  <strong>Doc. No.</strong>
                </td>
                <td colspan="5" align="left" valign="middle">
                  CHC-QC-Checklist     <xsl:value-of select="Docid" />
                </td>
              </tr>
              <tr>
                <td align="left" valign="middle">
                  <strong>Date </strong>
                </td>
                <td colspan="5" align="left" valign="middle">

                  <xsl:value-of select="Dateadded" />


                </td>
              </tr>
              <tr>
                <td colspan="13" rowspan="2" align="center" valign="middle" >
                  <strong>

                    DYED YARN TESTING REPORT (AS PER AQL -2.5, SPECIAL LEVEL 1)



                  </strong>
                </td>
                <td align="left" valign="middle" >
                  <strong>Version </strong>
                </td>
                <td colspan="5" align="left" valign="middle">03</td>
              </tr>
              <tr>
                <td align="left" valign="middle" >
                  <strong>Sys. Doc No </strong>
                </td>
                <td colspan="5" align="left" valign="middle">
                  <xsl:value-of select="DocNo" />
                </td>
              </tr>

            </xsl:if>

          </xsl:for-each>

          <tr>
            <td  align="center" valign="middle" rowspan="2">Date</td>
            <td  align="center" valign="middle" rowspan="2">
              Supplier
              name
            </td>
            <td  align="center" valign="middle" rowspan="2">
              Challan
              No &amp; Date
            </td>

            <td  align="center" valign="middle" rowspan="2">
            Lot No.
            </td>

            <td  align="center" valign="middle" rowspan="2">
        Tag No.
            </td>
            <td  align="center" valign="middle" rowspan="2">
              Yarn
              Type
            </td>
            <td  align="center" valign="middle" rowspan="2">
              Count
            </td>
            <td  align="center" valign="middle" rowspan="2">
              Total
              Bale
            </td>

            <td  align="center" valign="middle" rowspan="2">
              Sample
              Size
            </td>
            <td  align="center" valign="middle" rowspan="2">
              No.
              of Hank
            </td>

            <td  align="center" valign="middle"   rowspan="2">


              Shade No.


            </td>

            <td  align="center" valign="middle"   rowspan="2">


          Moisture Content 


            </td>
            <td  align="center" valign="middle" rowspan="2">

           Lab Test Color 


            </td>
            <td  align="center" valign="middle"  colspan="2">

              Lab Test Rubbing Fastness

            </td>
            <td  align="center" valign="middle"   rowspan="2">

              Recd. Qty


            </td>
            <td  align="center" valign="middle"  rowspan="2">

          Comments 


            </td>
            <td  align="center" valign="middle" >
              Result

            </td>
            <td  align="center" valign="middle" rowspan="2" >
              Lab
              Technician
            </td>
           
          </tr>

          <tr>
            <td  align="center" valign="middle"  >
       Dry


            </td>
            <td  align="center" valign="middle"  >
            Wet


            </td>
            <td  align="center" valign="middle"  >
              Pass/Fail


            </td>
          </tr>

          
          <report>
            <xsl:for-each select="/Report/Inspection">
              <inspection>
                <tr>
                  <td  valign="top">
                    <xsl:value-of select="./ReportDate" />
                  </td>
                  <td  valign="top">
                    <xsl:value-of select="./SupplierName" />
                  </td>
                  <td  align="left"
                      valign="middle" >

                    <xsl:value-of select="./ChallanNoDate" />
                  </td>

                  <td  align="left"
                      valign="middle" >

                    <xsl:value-of select="./LotNo" />
                  </td>

                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./TagNo" />
                  </td>

                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./YarnType" />
                  </td>
                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./COUNT" />
                  </td>
                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./TotalBale" />
                  </td>

                  <td  align="left"
                  valign="middle" >
                    -
                  </td>
                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./SampleSize" />
                  </td>
                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./NoofHank" />
                  </td>

                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./SHADENO" />
                  </td>
                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./MOISTURECONTENT" />
                  </td>

                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./LABTESTCOLOR" />
                  </td>
                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./DRY" />
                  </td>

                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./WET" />
                  </td>
                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./RECDQTY" />
                  </td>


                  <td  align="left"
                      valign="middle" >
                    <xsl:value-of select="./COMMENTS" />
                  </td>
                  <td  align="left" valign="middle" >
                    <xsl:value-of select="./APPROVALUSERNAME" />
                  </td>


             


               
                </tr>

              </inspection>
            </xsl:for-each>
          </report>
          <tr>
            <td colspan="19" valign="middle" style=""></td>
          </tr>
          <tr>
            <td colspan="19" valign="middle" style=""></td>
          </tr>





        </table>

      </body>
    </html>




  </xsl:template>
</xsl:stylesheet>
