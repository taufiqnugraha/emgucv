//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_OCL_C_H
#define EMGU_OCL_C_H

#include "opencv2/ocl.hpp"
#include "opencv2/core/types_c.h"
#include "opencv2/core/core_c.h"
#include "../emgu_c.h"

CVAPI(int) oclGetDevice(std::vector<cv::ocl::Info>* oclInfoVec, int devicetype);

//----------------------------------------------------------------------------
//
//  OclMat
//
//----------------------------------------------------------------------------

CVAPI(cv::ocl::oclMat*) oclMatCreateDefault();

CVAPI(cv::ocl::oclMat*) oclMatCreate(int rows, int cols, int type);

//CVAPI(cv::ocl::oclMat*) oclMatCreateContinuous(int rows, int cols, int type);

CVAPI(cv::ocl::oclMat*) oclMatCreateFromArr(CvArr* arr);

CVAPI(int) oclMatGetType(cv::ocl::oclMat* oclMat);

CVAPI(emgu::size) oclMatGetSize(cv::ocl::oclMat* oclMat);

CVAPI(emgu::size) oclMatGetWholeSize(cv::ocl::oclMat* oclMat);

CVAPI(bool) oclMatIsEmpty(cv::ocl::oclMat* oclMat);

CVAPI(bool) oclMatIsContinuous(cv::ocl::oclMat* oclMat);

CVAPI(int) oclMatGetChannels(cv::ocl::oclMat* oclMat);

CVAPI(void) oclMatRelease(cv::ocl::oclMat** mat);

//Pefroms blocking upload data to oclMat.
CVAPI(void) oclMatUpload(cv::ocl::oclMat* oclMat, CvArr* arr);

//Downloads data from device to host memory. Blocking calls.
CVAPI(void) oclMatDownload(cv::ocl::oclMat* oclMat, CvArr* arr);

CVAPI(int) oclCountNonZero(cv::ocl::oclMat* oclMat);

CVAPI(void) oclMatAdd(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatAddS(const cv::ocl::oclMat* a, const CvScalar scale, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatSubtract(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatSubtractS(const cv::ocl::oclMat* a, const CvScalar scale, cv::ocl::oclMat* c, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatMultiply(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, double scale);

CVAPI(void) oclMatMultiplyS(const cv::ocl::oclMat* a, const double s, cv::ocl::oclMat* c);

CVAPI(void) oclMatDivide(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, double scale);

//CVAPI(void) oclMatDivideSR(const cv::ocl::oclMat* a, const CvScalar s, cv::ocl::oclMat* c);

CVAPI(void) oclMatDivideSL(const double s, const cv::ocl::oclMat* b, cv::ocl::oclMat* c);

CVAPI(void) oclMatAddWeighted(const cv::ocl::oclMat* src1, double alpha, const cv::ocl::oclMat* src2, double beta, double gamma, cv::ocl::oclMat* dst);

CVAPI(void) oclMatAbsdiff(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c);

CVAPI(void) oclMatAbsdiffS(const cv::ocl::oclMat* a, const CvScalar s, cv::ocl::oclMat* c);

CVAPI(void) oclMatFlip(const cv::ocl::oclMat* a, cv::ocl::oclMat* b, int flipCode);

CVAPI(void) oclMatBitwiseNot(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst);

CVAPI(void) oclMatBitwiseAnd(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatBitwiseAndS(const cv::ocl::oclMat* src1, const cv::Scalar sc, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatBitwiseOr(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatBitwiseOrS(const cv::ocl::oclMat* src1, const cv::Scalar sc, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatBitwiseXor(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatBitwiseXorS(const cv::ocl::oclMat* src1, const cv::Scalar sc, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatErode( const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const CvArr* kernel, CvPoint anchor, int iterations, int borderType, cv::Scalar borderValue);

CVAPI(void) oclMatDilate( const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const CvArr* kernel, CvPoint anchor, int iterations, int borderType, cv::Scalar borderValue);

CVAPI(void) oclMatMorphologyEx( const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int op, const CvArr* kernel, CvPoint anchor, int iterations);

CVAPI(void) oclMatCompare(const cv::ocl::oclMat* a, const cv::ocl::oclMat* b, cv::ocl::oclMat* c, int cmpop);

CVAPI(void) oclMatCvtColor(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int code);

CVAPI(void) oclMatCopy(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatSetTo(cv::ocl::oclMat* mat, const CvScalar s, const cv::ocl::oclMat* mask);

CVAPI(void) oclMatResize(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, double fx, double fy, int interpolation);

//only support single channel oclMat
CVAPI(void) oclMatMinMaxLoc(const cv::ocl::oclMat* src, 
   double* minVal, double* maxVal, 
   CvPoint* minLoc, CvPoint* maxLoc, 
   const cv::ocl::oclMat* mask);

CVAPI(void) oclMatMatchTemplate(const cv::ocl::oclMat* image, const cv::ocl::oclMat* templ, cv::ocl::oclMat* result, int method, cv::ocl::MatchTemplateBuf* buffer);

CVAPI(void) oclMatPyrDown(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst);

CVAPI(void) oclMatPyrUp(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst);

CVAPI(void) oclMatSplit(const cv::ocl::oclMat* src, cv::ocl::oclMat** dst);

CVAPI(void) oclMatMerge(const cv::ocl::oclMat** src, cv::ocl::oclMat* dst);

CVAPI(void) oclMatConvertTo(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, double alpha, double beta);

CVAPI(void) oclMatFilter2D(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, const CvArr* kernel, CvPoint anchor, int borderType);

CVAPI(void) oclMatReshape(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int cn, int rows);

CVAPI(void) oclMatSobel(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int dx, int dy, int ksize, double scale, int borderType);

CVAPI(void) oclMatGaussianBlur(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, CvSize ksize, double sigma1, double sigma2, int borderType);

CVAPI(void) oclMatLaplacian(const cv::ocl::oclMat* src, cv::ocl::oclMat* dst, int ksize, double scale);

CVAPI(void) oclMatGemm(const cv::ocl::oclMat* src1, const cv::ocl::oclMat* src2, double alpha, 
   const cv::ocl::oclMat* src3, double beta, cv::ocl::oclMat* dst, int flags);

CVAPI(void) oclMatCanny(const cv::ocl::oclMat* image, cv::ocl::oclMat* edges, double lowThreshold, double highThreshold, int apertureSize, bool L2gradient);


//----------------------------------------------------------------------------
//
//  OclHOGDescriptor
//
//----------------------------------------------------------------------------
CVAPI(void) oclHOGDescriptorGetPeopleDetector64x128(std::vector<float>* vector);

CVAPI(void) oclHOGDescriptorGetPeopleDetector48x96(std::vector<float>* vector);

CVAPI(cv::ocl::HOGDescriptor*) oclHOGDescriptorCreateDefault();

CVAPI(cv::ocl::HOGDescriptor*) oclHOGDescriptorCreate(
   cv::Size* _winSize, 
   cv::Size* _blockSize, 
   cv::Size* _blockStride,
   cv::Size* _cellSize, 
   int _nbins, 
   double _winSigma,
   double _L2HysThreshold, 
   bool _gammaCorrection, 
   int _nlevels);

CVAPI(void) oclHOGSetSVMDetector(cv::ocl::HOGDescriptor* descriptor, std::vector<float>* vector);

CVAPI(void) oclHOGDescriptorRelease(cv::ocl::HOGDescriptor** descriptor);

CVAPI(void) oclHOGDescriptorDetectMultiScale(
   cv::ocl::HOGDescriptor* descriptor, 
   cv::ocl::oclMat* img, 
   CvSeq* foundLocations,
   double hitThreshold, 
   CvSize winStride,
   CvSize padding, 
   double scale,
   int groupThreshold);

//----------------------------------------------------------------------------
//
//  GpuMatchTemplateBuf
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::MatchTemplateBuf*) oclMatchTemplateBufCreate();
CVAPI(void) oclMatchTemplateBufRelease(cv::ocl::MatchTemplateBuf** buffer);

//----------------------------------------------------------------------------
//
//  GpuCascadeClassifier
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::OclCascadeClassifierBuf*) oclCascadeClassifierCreate(const char* filename);

CVAPI(void) oclCascadeClassifierRelease(cv::ocl::OclCascadeClassifierBuf** classifier);

//----------------------------------------------------------------------------
//
//  oclPyrLKOpticalFlow
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::PyrLKOpticalFlow*) oclPyrLKOpticalFlowCreate(cv::Size winSize, int maxLevel, int iters, bool useInitialFlow);
CVAPI(void) oclPyrLKOpticalFlowSparse(
   cv::ocl::PyrLKOpticalFlow* flow, 
   const cv::ocl::oclMat* prevImg, 
   const cv::ocl::oclMat* nextImg, 
   const cv::ocl::oclMat* prevPts, 
   cv::ocl::oclMat* nextPts,
   cv::ocl::oclMat* status, 
   cv::ocl::oclMat* err);
CVAPI(void) oclPyrLKOpticalFlowDense(
   cv::ocl::PyrLKOpticalFlow* flow, 
   const cv::ocl::oclMat* prevImg, 
   const cv::ocl::oclMat* nextImg,
   cv::ocl::oclMat* u, 
   cv::ocl::oclMat* v, 
   cv::ocl::oclMat* err);
CVAPI(void) oclPyrLKOpticalFlowRelease(cv::ocl::PyrLKOpticalFlow** flow);

//----------------------------------------------------------------------------
//
//  Ocl Stereo
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::StereoBM_OCL*) oclStereoBMCreate(int preset, int ndisparities, int winSize);

CVAPI(void) oclStereoBMFindStereoCorrespondence(cv::ocl::StereoBM_OCL* stereo, const cv::ocl::oclMat* left, const cv::ocl::oclMat* right, cv::ocl::oclMat* disparity);

CVAPI(void) oclStereoBMRelease(cv::ocl::StereoBM_OCL** stereoBM);

CVAPI(cv::ocl::StereoConstantSpaceBP*) oclStereoConstantSpaceBPCreate(int ndisp, int iters, int levels, int nr_plane);

CVAPI(void) oclStereoConstantSpaceBPFindStereoCorrespondence(cv::ocl::StereoConstantSpaceBP* stereo, const cv::ocl::oclMat* left, const cv::ocl::oclMat* right, cv::ocl::oclMat* disparity);

CVAPI(void) oclStereoConstantSpaceBPRelease(cv::ocl::StereoConstantSpaceBP** stereoBM);

//----------------------------------------------------------------------------
//
//  OclBruteForceMatcher
//
//----------------------------------------------------------------------------
CVAPI(cv::ocl::BFMatcher_OCL*) oclBruteForceMatcherCreate(int distType);

CVAPI(void) oclBruteForceMatcherRelease(cv::ocl::BFMatcher_OCL** matcher);

CVAPI(void) oclBruteForceMatcherAdd(cv::ocl::BFMatcher_OCL* matcher, const cv::ocl::oclMat* trainDescs);

CVAPI(void) oclBruteForceMatcherKnnMatchSingle(
   cv::ocl::BFMatcher_OCL* matcher,
   const cv::ocl::oclMat* queryDescs, const cv::ocl::oclMat* trainDescs,
   cv::ocl::oclMat* trainIdx, cv::ocl::oclMat* distance, 
   int k, const cv::ocl::oclMat* mask);

//----------------------------------------------------------------------------
//
//  Vector of VectorOfOclInfo
//
//----------------------------------------------------------------------------
CVAPI(std::vector<cv::ocl::Info>*) VectorOfOclInfoCreate();

CVAPI(std::vector<cv::ocl::Info>*) VectorOfOclInfoCreateSize(int size);

CVAPI(int) VectorOfOclInfoGetSize(std::vector<cv::ocl::Info>* v);

CVAPI(void) VectorOfOclInfoClear(std::vector<cv::ocl::Info>* v);

CVAPI(void) VectorOfOclInfoRelease(std::vector<cv::ocl::Info>* v);

CVAPI(cv::ocl::Info*) VectorOfOclInfoGetStartAddress(std::vector<cv::ocl::Info>* v);

CVAPI(cv::ocl::Info*) VectorOfOclInfoGetItem(std::vector<cv::ocl::Info>* v, int index);

//----------------------------------------------------------------------------
//
//  OclInfo
//
//----------------------------------------------------------------------------
CVAPI(const char*) oclInfoGetPlatformName(cv::ocl::Info* oclInfo);

CVAPI(int) oclInfoGetDeviceCount(cv::ocl::Info* oclInfo);

CVAPI(const char*) oclInfoGetDeviceName(cv::ocl::Info* oclInfo, int index);

#endif