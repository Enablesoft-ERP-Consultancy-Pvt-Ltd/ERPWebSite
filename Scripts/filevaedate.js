
function isImage(file)
	{
	//Get a file extension
	var ext = file.substr(file.lastIndexOf('.')).toLowerCase()
	//Check extension to image types.
	return '.gif,.jpg,.png,.jpeg,.bmp,'.indexOf(ext+',') >= 0
	};

	var lastfieldname = ''
	var filenamechecked = ''
			
function preview(htmlupload,lablename,previewsrc,HIDIMG) {
  
	//get current input preview
	var filename = htmlupload;
	var HIDFILE = HIDIMG;
	var preimg = previewsrc;
	var lablname = lablename;
	var htmlfile = document.getElementById(filename);
	var file = htmlfile.value

	//set the size field.
	var himg = document.getElementById(HIDFILE);
	if (file.length>0) himg.src = 'file://' + file;
	else {himg.src = '';};

	if (file.length<=0) return;
	var ipreview = document.getElementById(preimg);

	if (isImage(file))
	{
		//Show preview for the image.
		ipreview.src = 'file://' + file
		ipreview.title = 'Image ' + file
		if (ipreview.width != 70) ipreview.width = 70;
			} 
		else
			{
			//some default image for preview
			alert('Please choose some image file (.gif,.jpg,.png,.jpeg,.bmp)');
			ipreview.src = '../images/error1.jpg'
			ipreview.width = 30;
			var size = document.getElementById(lablname);
			size.innerHTML = "error!";
			}

		lastfieldname = htmlfile.name
	}

//this function gets a sizes of images

	var maxFileSize = 25600

	function checkFileSize(filehid,lablename,previewsrc) {
	
	var labnm = lablename;
	var imgsrc = filehid;
	var imgpre = previewsrc;
	
	var totalSize = 0;
	var imageSize;
	var overLimit = false;
	
	var himg = document.getElementById(imgsrc);
	var size = document.getElementById(labnm);
	var fileSize = himg.fileSize ;
	fileSize = parseInt(fileSize);
	if (fileSize <= 0)
	{
	alert("Size cannot be less than 0");
	} 
	else
	{
		imageSize = formatSize(fileSize);
		if ( fileSize>maxFileSize )
		{
		size.innerHTML = imageSize;
		alert("Image should be less than or equal to "+ formatSize(maxFileSize)+ " in size");
		var errImg = document.getElementById(imgpre);
		errImg.src = '../images/error1.jpg'
		errImg.width = 30;
		size.innerHTML = "error!"
		}
		else
		{
		size.innerHTML = imageSize;
		}
	}
	}

	function formatSize(size)
	{
	if (size < 0x100000)
		{// < 1 MB
		return Math.round(size / 0x400)+"KB";
	}
	}
