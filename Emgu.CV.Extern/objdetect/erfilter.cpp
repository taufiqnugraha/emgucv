//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "objdetect_c.h"

//ERFilter
cv::ERFilter* CvERFilterNM1Create(
   cv::String* classifier, 
   int thresholdDelta, 
   float minArea,
   float maxArea, 
   float minProbability,
   bool nonMaxSuppression,
   float minProbabilityDiff)
{
   cv::Ptr<cv::ERFilter> filter = cv::createERFilterNM1(cv::loadClassifierNM1(*classifier), thresholdDelta, minArea, maxArea, minProbability, nonMaxSuppression, minProbabilityDiff);
   filter.addref();
   return filter.get();
}
cv::ERFilter* CvERFilterNM2Create(cv::String* classifier, float minProbability)
{
   cv::Ptr<cv::ERFilter> filter = cv::createERFilterNM2(cv::loadClassifierNM2(*classifier), minProbability);
   filter.addref();
   return filter.get();

}
void CvERFilterRelease(cv::ERFilter** filter)
{
   delete *filter;
   *filter = 0;
}
void CvERFilterRun(cv::ERFilter* filter, cv::_InputArray* image, std::vector<cv::ERStat>* regions)
{
   filter->run(*image, *regions);
}

void CvERGrouping(cv::_InputArray* channels, std::vector<cv::ERStat>** regions, int count, cv::String* fileName, float minProbability, std::vector<cv::Rect>* groups)
{
   std::vector< std::vector< cv::ERStat > > statVecs;
   for (int i = 0; i < count; i++)
   {
      statVecs.push_back(*regions[i]);
   }
   
   cv::erGrouping(*channels, statVecs, *fileName, minProbability, *groups);

}